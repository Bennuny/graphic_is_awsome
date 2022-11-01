## Transformation Cont.

### 1. Rotation
```
旋转的逆等于旋转的转置
```
$$
R_{-\theta} = {R_{-\theta}}^{-1}
$$
**正交矩阵**：如果一个矩阵的逆等于一个矩阵的转置

### 2. 3D transformation
- Viewing(观测) transformation
- Projection(投影) transformation
  - Orthographic() projection
  - Perspective() projection

Rotation around x-, y- or z-axis
$$
R_x(\alpha) = 
\begin{pmatrix}
1 & 0 & 0 & 0 \\
0 & \cos(\alpha) & -\sin(\alpha) & 0 \\
0 & \sin(\alpha) & \cos(\alpha) & 0 \\
0 & 0 & 0 & 1 \\   
\end{pmatrix}
$$

$$
R_y(\alpha) = 
\begin{pmatrix}
\cos(\alpha) & 0 & \sin(\alpha) & 0 \\
0 & 1 & 0 & 0 \\
-\sin(\alpha) & 0 & \cos(\alpha) & 0 \\
0 & 0 & 0 & 1 \\   
\end{pmatrix}
$$
*Anthing strange about Y. Cross Product*

$$
R_z(\alpha) = 
\begin{pmatrix}
\cos(\alpha) & -\sin(\alpha) & 0 & 0 \\
\sin(\alpha) & \cos(\alpha) & 0 & 0 \\
0 & 0 & 1 & 0 \\
0 & 0 & 0 & 1 \\   
\end{pmatrix}
$$

### Compose any 3D rotation from R_x, R_y, R_z
$$
R_{xyz}(\alpha\theta\gamma) = R_{x}(\alpha)R_{y}(\theta)R_{z}(\gamma)
$$

### Rodrigues` Rotation Formula

### View / Camera Transformation
-  What is view transformation?
-  Think about how to take a photo
   -  Find a good place and arrange people (model transformation)
   -  Find a good "angle" to put the camera (view transformation)
   -  Cheese! (projection transformation)

### How to perform view transformation?
- Define the camera first
  - Positioin 
  - Look-at / gaze direction
  - Up direction (assuming perp. to look-at)
- Key observation
  - If the camera and all objects move together, the "photo" will be the same. 
  - <font color="red">How about that we always transform the camera to</font>
    - The origin, up at Y, look at -Z
    - And transform the objects along with camera
- Mview in math?
  - Translate e to origin
  - Rotates g t0 -Z
  - Rotates to Y
  - Rotates (g cross t) t0 X
  - Diffcult to write!


### Perspective Projection
- Recall: property of homogeneous coordinates
  - (x, y, z, 1), (kx, ky, kz, k!=0), (xz, yz, zz, z!=0) all represent the same point (x, y, z) in 3D
  - e.g. (1, 0, 0, 1) and (2, 0, 0, 2) both represent(1, 0, 0)

### What`s near plane`s l, r, b, t then?
- Sometimes people prefer: vertical field-of-view (fovY) and aspect ratio (assume symmetry i.e. l = -r, b = -t)

***problem.***
1. 四元数、欧拉角?
2. OpenGL使用左手系
3. 作业0~