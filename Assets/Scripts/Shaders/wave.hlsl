//UNITY_SHADER_NO_UPGRADE
#ifndef MYHLSLINCLUDE_INCLUDED
#define MYHLSLINCLUDE_INCLUDED



float3 mod289(float3 x) {
    return x - floor(x * (1.0 / 289.0)) * 289.0;
}

float2 mod289(float2 x) {
    return x - floor(x * (1.0 / 289.0)) * 289.0;
}

float3 permute(float3 x) {
    return mod289((x * 34.0 + 1.0) * x);
}

float3 taylorInvSqrt(float3 r) {
    return 1.79284291400159 - 0.85373472095314 * r;
}

// output noise is in range [-1, 1]
float snoise(float2 v) {
    const float4 C = float4(0.211324865405187,  // (3.0-sqrt(3.0))/6.0
                            0.366025403784439,  // 0.5*(sqrt(3.0)-1.0)
                            -0.577350269189626, // -1.0 + 2.0 * C.x
                            0.024390243902439); // 1.0 / 41.0

    // First corner
    float2 i  = floor(v + dot(v, C.yy));
    float2 x0 = v -   i + dot(i, C.xx);

    // Other corners
    float2 i1;
    i1.x = step(x0.y, x0.x);
    i1.y = 1.0 - i1.x;

    // x1 = x0 - i1  + 1.0 * C.xx;
    // x2 = x0 - 1.0 + 2.0 * C.xx;
    float2 x1 = x0 + C.xx - i1;
    float2 x2 = x0 + C.zz;

    // Permutations
    i = mod289(i); // Avoid truncation effects in permutation
    float3 p =
      permute(permute(i.y + float3(0.0, i1.y, 1.0))
                    + i.x + float3(0.0, i1.x, 1.0));

    float3 m = max(0.5 - float3(dot(x0, x0), dot(x1, x1), dot(x2, x2)), 0.0);
    m = m * m;
    m = m * m;

    // Gradients: 41 points uniformly over a line, mapped onto a diamond.
    // The ring size 17*17 = 289 is close to a multiple of 41 (41*7 = 287)
    float3 x = 2.0 * frac(p * C.www) - 1.0;
    float3 h = abs(x) - 0.5;
    float3 ox = floor(x + 0.5);
    float3 a0 = x - ox;

    // Normalise gradients implicitly by scaling m
    m *= taylorInvSqrt(a0 * a0 + h * h);

    // Compute final noise value at P
    float3 g = float3(
        a0.x * x0.x + h.x * x0.y,
        a0.y * x1.x + h.y * x1.y,
        g.z = a0.z * x2.x + h.z * x2.y
    );
    return 130.0 * dot(m, g);
}

float snoise01(float2 v) {
    return snoise(v) * 0.5 + 0.5;
}

float circle(float2 st, float2 center, float radius, float bleed) {
    return smoothstep(1., 1. - bleed, distance(st, center) / radius);
}

float ring(float2 st, float2 center, float radius, float thickness, float bleed1, float bleed2) {
	return circle(st, center, radius, bleed1) - circle(st, center, radius - thickness, bleed2);
}


void main_float(float uTime, float2 uFragCoord, float uFreq, float uThickness, float2 uOrigin, float uRadius, float uBleed, out float4 Out)
{
    float2 uv = float2(uFragCoord.x, uFragCoord.y);

    
    
	float time = uTime % uFreq;
	float noiseTime = uTime * 2.;

	float r = uRadius;
	float a = atan2(uv.y, uv.x);
	float noiseA = a + noiseTime;

    float2 nPos = float2(cos(uv.x*5.), sin(uv.y*5.));

    float n = snoise01(nPos);
	float n2 = snoise01(nPos + noiseTime);


    r +=   n*0.18;
    r +=  n2*0.08;

	r *= time*3.;
        

	float pct = ring(uv, uOrigin, r, uThickness*time, uBleed, uBleed); 
    float3 color = float3(1., 1.,1.) * pct + float3(1., 1., 1.) * pct * n * 2.;
    Out = float4(color,1.0);

}

#endif //MYHLSLINCLUDE_INCLUDED