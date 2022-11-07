## Shading 1 (Illumination, Shading and Graphics Pipeline)

### Visible / occlusion
- Z-buffering

### Shading
- Illumination & Shading
- Graphics Pipeline

### Painter`s Algorithm
Inspired by how painters paint  
Paint from back to front, overwrite in the framebuffer.（油画家，层层覆盖）

### Z-Buffer
This is the algorithm that eventually won.  
Idea:
- Store current min. z-value for <u>each</u> sample (pixel)
- Needs an additional buffer for depth values
  - frame buffer stores color values
  - depth buffer (z-buffer) stores depth

```
  for (each triangle T)
    for (each sample (x,y,z) in T)
      if (z < zbuffer[x, y])      // closest sample so far
        framebuffer[x,y] = rgb;   // update color
        zbuffer[x,y] = z;         // updater z-buffer
      else
        ...
```

### Z-Buffer Complexity
- O(n) for n triangles (assuming constant coverage)
- How is it possible to sort n triangles in linear time?
- Drawing triangles in differents orders?
Most important visibility algorithm
- Implemented in hardware for all GPUs

### Shading: Definition
- In this course  
The process of applying a material to an object. 不同物品应用不同材质~

### A Simple Shading Model (Blinn-Phong Reflectance Model)
- Specular highlights   镜面反射
- Diffuse reflection    漫反射
  - Light is scattered uniformly in all directions
    - Surface color is the same for all viewing direction
  - But how much light (energy) is received
    - *Lambert`s cosine law*
- Ambient lighting      环境光照

### Shading is Local [53`]
Compute light reflected toward camera at a specific <font color='red'> shading point </font>.  
Inputs:
- shading point
- Viewer direction, v
- Surface normal, n
- Light direction, l (for each of many lights)
- Surface parameters (color, shininess)

No shadows will be generated! (shading != shadow)
$$
L_d = k_d(I/r^2)max(0,n·1)
$$
Ld: diffuselyreflected light
kd: diffuse coefficient (color)
I/r^2: energy arrived at the shading point
max(0, n·1): energy received by the shading point
