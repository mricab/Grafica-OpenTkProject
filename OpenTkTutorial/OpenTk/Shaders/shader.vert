#version 330 core

layout (location = 0) in vec4 VertInPosition;
layout (location = 1) in vec4 VertInColor;
layout (location = 2) in vec2 VertInTextureCoordinates;

out vec4 FragInColor;
out vec2 FragInTextureCoordinates;

uniform mat4 TransformMatrix;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    gl_Position = VertInPosition * model * view * projection;
    FragInColor = VertInColor;
    FragInTextureCoordinates = VertInTextureCoordinates;
}