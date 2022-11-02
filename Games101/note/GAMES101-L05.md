## Rasterization.

### What`s after MVP?
- Model transformation (placing objects)
- View transformation (placing camera)
- Projection transformation
  - Orthographic projection (cuboid to "canonical" cube [-1, 1]^3)
  - Perspective projection (frustum to "canonical" cube)
- Canonical cube to ?

### Canonical Cube to Screen? 📝
- What is screen?
  - An array of pixels. 1920*1080 (1080P) 1080*720(720P)
  - Size of the array: resolution
  - A typical kind of raster display
- Raster == screen in German
  - Rasterize == drawing onto the screen
- Pixel (FYI, short of "picture element")
  - For now: A pixel is a little square with uniform color
  - Color is a mixture of ( <font color='red'> red </font>, <font color='green'> green </font>, <font color='blue'> blue </font>) 

- Viewport transform matrix:
$$
M_{viewport} = 
\begin{pmatrix} 
\frac {width}{2} & 0 & 0 & \frac {width}{2} \\ 
0 & \frac {height}{2} & 0 & \frac {height}{2} \\
0 & 0 & 0 & 0 \\
0 & 0 & 0 & 1
\end{pmatrix}
$$

## Drawing Machines
- 电子墨水瓶刷新用时比较久；