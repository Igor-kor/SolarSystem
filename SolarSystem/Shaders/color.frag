#version 330 core

out vec4 gl_FragColor;

uniform vec4 Scolor;

void main()
{
    gl_FragColor  = Scolor;
}