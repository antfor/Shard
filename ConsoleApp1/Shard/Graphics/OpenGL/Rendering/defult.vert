﻿#version 330 core
layout (location = 0) in vec3 aPos;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

uniform mat4 mvp;

void main()
{
    gl_Position = vec4(aPos.x, aPos.y, aPos.z, 1.0) * mvp;
}