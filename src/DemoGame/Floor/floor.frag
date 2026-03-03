#version 330 core

uniform float scale;
uniform float size;

in vec4 fragPos;

///////////////////////////////////////////////////////////////////////////////
// Output color
///////////////////////////////////////////////////////////////////////////////
layout(location = 0) out vec4 fragmentColor;

float filterwidth(vec2 v)
{
  vec2 fw = max(abs(dFdx(v)), abs(dFdy(v)));
  return max(fw.x, fw.y);
}

vec2 BUMPINT(vec2 x){

    return (floor((x)/2.0) + 2.0 * max(((x)/2.0) - floor((x)/2.0) - .5, 0.0));
}

float checker(vec2 uv)
{
  float width = filterwidth(uv);
  vec2 p0 = uv - .5 * width, p1 = uv + .5 * width;
  
  vec2 i = (BUMPINT(p1) - BUMPINT(p0)) / width;
  return i.x * i.y + (1.0 - i.x) * (1.0 - i.y);
}


void main()
{
    float col = 0.1f + 0.05f * checker(fragPos.xz*scale/size);
	
    vec3 final_color = vec3(col);


    fragmentColor.w = 1.f;
    final_color = pow(final_color, vec3(1.0f/2.2f));
    fragmentColor.xyz = final_color;


	// Check if we got invalid results in the operation
	if(any(isnan(final_color)))
	{
		fragmentColor = vec4(1.f, 0.f, 1.f, 1.f);
	}

}
