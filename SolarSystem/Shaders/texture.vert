#version 330 core

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 normal;
layout (location = 2) in vec2 texCoord;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

uniform vec3 position;

out vec2 fragTexCoord;

void main()
{
    gl_Position = vec4( position + aPosition, 1.0) * model * view * projection;
    fragTexCoord = texCoord;
}