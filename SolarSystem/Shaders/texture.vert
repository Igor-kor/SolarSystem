#version 330 core

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 normal;
layout (location = 2) in vec2 texCoord;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;


out vec2 fragTexCoord;

void main()
{
    gl_Position = vec4( aPosition, 1.0) * model * view * projection;
    fragTexCoord = texCoord;
}