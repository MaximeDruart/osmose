/* Copyright (c) 2020 ExT (V.Sigalkin) */

using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;

using Random = UnityEngine.Random;

namespace extOSC.Examples
{
    public class SampleScript : MonoBehaviour
    {
        #region Static Public Methods

        public static int RemapValue(float value, float inputMin, float inputMax, int outputMin, int outputMax)
        {
            return (int)((value - inputMin) / (inputMax - inputMin) * (outputMax - outputMin) + outputMin);
        }

        #endregion

        #region Public Vars

        [Header("OSC Settings")]
        public OSCReceiver Receiver;

        public OSCTransmitter Transmitter;

        [Header("Transmitter UI Settings")]

        public Text TransmitterAddressBool;

        public Text TransmitterAddressFloat;

        public Text TransmitterTextBool;

        public Text TransmitterTextFloat;


        public Image TransmitterImageColor;

        [Header("Receiver UI Settings")]
        public Text ReceiverTextBlob;

        public InputField ReceiverInputChar;

        public Text ReceiverTextDouble;

        public Slider ReceiverSliderDouble;

        public Text ReceiverTextBool;

        public Toggle ReceiverToggleBool;

        public Text ReceiverTextFloat;

        public Slider ReceiverSliderFloat;

        public Image ReceiverImageImpulse;

        public Text ReceiverTextInt;

        public Slider ReceiverSliderInt;

        public Text ReceiverTextLong;

        public Slider ReceiverSliderLong;

        public Image ReceiverImageNull;

        public InputField ReceiverInputString;

        public InputField ReceiverInputTimeTagMonth;

        public InputField ReceiverInputTimeTagDay;

        public InputField ReceiverInputTimeTagYear;

        public InputField ReceiverInputTimeTagHour;

        public InputField ReceiverInputTimeTagMinute;

        public InputField ReceiverInputTimeTagSecond;

        public InputField ReceiverInputTimeTagMillisecond;

        public InputField ReceiverInputMidiChannel;

        public InputField ReceiverInputMidiStatus;

        public InputField ReceiverInputMidiData1;

        public InputField ReceiverInputMidiData2;

        public Image ReceiverImageColor;

        public Color BlinkImage;

        #endregion

        #region Private Vars

        private const string _floatAddress = "/example/2/float";

        private DateTime _timeTag;

        private OSCMidi _midi;

        private Color _defaultImageColor;

        private Dictionary<Image, Coroutine> _blinkCoroutinePool = new Dictionary<Image, Coroutine>();

        #endregion

        #region Unity Methods

        public void Start()
        {

            TransmitterAddressFloat.text = string.Format("<color=grey>{0}</color>", _floatAddress);
            Receiver.Bind(_floatAddress, ReceiveFloat);
        }

        #endregion

        #region Public Methods

        // TRANSMITTER

        public void SendFloat(float value)
        {
            Send(_floatAddress, OSCValue.Float(value));

            TransmitterTextFloat.text = value.ToString();
        }


        // RECEIVER
        public void ReceiveBlob(OSCMessage message)
        {
            if (message.ToBlob(out var value))
            {
                var bytesString = string.Empty;

                for (var index = 0; index < value.Length; index++)
                {
                    bytesString += $"0x{value[index]:x2}".ToUpper() + " ";
                }

                ReceiverTextBlob.text = bytesString.Remove(bytesString.Length - 1);
            }
        }

        public void ReceiveChar(OSCMessage message)
        {
            if (message.ToChar(out var value))
            {
                ReceiverInputChar.text = value.ToString();
                ;
                ReceiverInputChar.Select();
            }
        }

        public void ReceiveColor(OSCMessage message)
        {
            if (message.ToColor(out var value))
            {
                ReceiverImageColor.color = value;
            }
        }

        public void ReceiveDouble(OSCMessage message)
        {
            if (message.ToDouble(out var value))
            {
                ReceiverTextDouble.text = value.ToString();
                ReceiverSliderDouble.value = (float)value;
            }
        }

        public void ReceiveBool(OSCMessage message)
        {
            if (message.ToBool(out var value))
            {
                ReceiverTextBool.text = value.ToString();
                ReceiverToggleBool.isOn = value;
            }
        }

        public void ReceiveFloat(OSCMessage message)
        {
            if (message.ToFloat(out var value))
            {
                ReceiverTextFloat.text = value.ToString();
                ReceiverSliderFloat.value = value;
            }
        }

        public void ReceiveImpulse(OSCMessage message)
        {
            if (message.HasImpulse())
            {
                ImageBlink(ReceiverImageImpulse);
            }
        }

        public void ReceiveInt(OSCMessage message)
        {
            int value;
            if (message.ToInt(out value))
            {
                ReceiverTextInt.text = value.ToString();

                var floatValue = (float)(value / (double)int.MaxValue);
                ReceiverSliderInt.value = floatValue;
            }
        }

        public void ReceiveLong(OSCMessage message)
        {
            if (message.ToLong(out var value))
            {
                ReceiverTextLong.text = value.ToString();

                var floatValue = (float)(value / (double)long.MaxValue);
                ReceiverSliderLong.value = floatValue;
            }
        }

        public void ReceiveNull(OSCMessage message)
        {
            if (message.HasNull())
            {
                ImageBlink(ReceiverImageNull);
            }
        }

        public void ReceiveString(OSCMessage message)
        {
            if (message.ToString(out var value))
            {
                ReceiverInputString.text = value;
            }
        }

        public void ReceiveTimeTag(OSCMessage message)
        {
            if (message.ToTimeTag(out var value))
            {
                ReceiverInputTimeTagMonth.text = value.Month.ToString();
                ReceiverInputTimeTagDay.text = value.Day.ToString();
                ReceiverInputTimeTagYear.text = value.Year.ToString();
                ReceiverInputTimeTagHour.text = value.Hour.ToString();
                ReceiverInputTimeTagMinute.text = value.Minute.ToString();
                ReceiverInputTimeTagSecond.text = value.Second.ToString();
                ReceiverInputTimeTagMillisecond.text = value.Millisecond.ToString();
            }
        }

        public void ReceiveMidi(OSCMessage message)
        {
            if (message.ToMidi(out var value))
            {
                ReceiverInputMidiChannel.text = value.Channel.ToString();
                ReceiverInputMidiStatus.text = value.Status.ToString();
                ReceiverInputMidiData1.text = value.Data1.ToString();
                ReceiverInputMidiData2.text = value.Data2.ToString();
            }
        }

        #endregion

        #region Private Methods

        private void Send(string address, OSCValue value)
        {
            var message = new OSCMessage(address, value);

            Transmitter.Send(message);
        }

        private void ImageBlink(Image image)
        {
            if (_blinkCoroutinePool.ContainsKey(image))
            {
                StopCoroutine(_blinkCoroutinePool[image]);

                _blinkCoroutinePool.Remove(image);
            }

            _blinkCoroutinePool.Add(image, StartCoroutine(ImageBlinkCoroutine(image)));
        }

        private IEnumerator ImageBlinkCoroutine(Image image)
        {
            var blinkTimer = 0f;
            var blinkDuration = 0.05f;

            while (blinkTimer < blinkDuration)
            {
                blinkTimer += Time.deltaTime;

                image.color = Color.Lerp(image.color, BlinkImage, blinkTimer / blinkDuration);

                yield return null;
            }

            blinkTimer = 0;
            blinkDuration = 0.2f;

            while (blinkTimer < blinkDuration)
            {
                blinkTimer += Time.deltaTime;

                image.color = Color.Lerp(BlinkImage, _defaultImageColor, blinkTimer / blinkDuration);

                yield return null;
            }

            if (_blinkCoroutinePool.ContainsKey(image))
                _blinkCoroutinePool.Remove(image);
        }

        #endregion
    }
}