#version 330 core
layout (location = 0) in vec3 aPos;

uniform mat4 mvp;

out vec4 fragPos;

void main()
{
    fragPos = vec4(aPos, 1);
	gl_Position = mvp * vec4(aPos, 1);
}