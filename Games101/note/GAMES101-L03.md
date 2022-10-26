## Transformation

### Scale
$$
\begin{bmatrix} 
\dot{x} \\ 
\dot{y} \\
\end{bmatrix}
=
\begin{bmatrix} 
s_x & 0 \\ 
0 & s_y \\
\end{bmatrix}
\begin{bmatrix} 
x \\ 
y \\
\end{bmatrix}
$$

### Reflection Matrx
$$ \dot{x} = -x $$
$$ \dot{y} = y $$

### Shear Matrix
```
Hints:
   Horizontal shift is 0 at y = 0
   Horizontal shift is a at y = 1
   Vertical shift is always 0
```
$$
\begin{bmatrix} 
\dot{x} \\ 
\dot{y} \\
\end{bmatrix}
=
\begin{bmatrix} 
1 & a \\ 
0 & 1 \\
\end{bmatrix}
\begin{bmatrix} 
x \\ 
y \\
\end{bmatrix}
$$

### Rotate (about the origin(0, 0), CCW by default)  
· Rotate Matrix
$$ R_\theta = 
\begin{bmatrix} 
\cos(\theta) & -\sin(\theta) \\ 
\sin(\theta) & \cos(\theta) \\
\end{bmatrix}
$$

· Linear Transform = Matrix

### **Homogeneous coordinates**
- Translation *cannot* be represented in matrix from
$$
\begin{bmatrix} 
\dot{x} \\ 
\dot{y} \\
\end{bmatrix}
=
\begin{bmatrix} 
a & b \\ 
c & d \\
\end{bmatrix}
\begin{bmatrix} 
x \\ 
y \\
\end{bmatrix}
+
\begin{bmatrix} 
t_x \\ 
t_y \\
\end{bmatrix}
$$
<center><font color='red'> (So, translation is NOT linear transform!) </font></center>

- But we don`t want translation to be a special case
- Is there a unified way to represent all transformations? (and what`s the cost)

Add a third coordinate (w-coordinate)<font color='red'> (向量平移不变) </font>
- $$ 2D \quad point = (x, y, 1)^T $$
- $$ 2D \quad vector = (x, y, 0)^T $$
**Matrix representation of translations**
$$
\begin{pmatrix} 
\dot{x} \\ 
\dot{y} \\
\dot{w} \\
\end{pmatrix}
=
\begin{pmatrix} 
1 & 0 & t_x \\ 
0 & 1 & t_y \\
0 & 0 & 1 \\
\end{pmatrix}
\begin{pmatrix} 
x \\ 
y \\
1 \\
\end{pmatrix}
=
\begin{pmatrix} 
x + t_x \\ 
y + t_y \\
1 \\
\end{pmatrix}
$$

Valid operation if w-coordinate of result is 1 or 0
```
- vector + vector = vector
- point - point = vector
- point + vector = point
- point + point = ??
```

### Affine Transformations
Affine map = linear map + translation
$$
\begin{pmatrix} 
\dot{x} \\ 
\dot{y} \\
\end{pmatrix}
=
\begin{pmatrix} 
a & b\\ 
c & d\\
\end{pmatrix}
·
\begin{pmatrix} 
x \\ 
y \\
\end{pmatrix}
+
\begin{pmatrix} 
tx \\ 
ty \\
\end{pmatrix}
$$
Using homogenous coordinates:
$$
\begin{pmatrix} 
\dot{x} \\ 
\dot{y} \\
\dot{w} \\
\end{pmatrix}
=
\begin{pmatrix} 
a & b & t_x \\ 
c & d & t_y \\
0 & 0 & 1 \\
\end{pmatrix}
\begin{pmatrix} 
x \\ 
y \\
1 \\
\end{pmatrix}
$$

### Inverse Transform
$$ M^{-1} $$

### Transform Ordering Matters!
**Matrix multiplication is <u>not</u> commutative**
$$ R_{45} · T_{1,0} \neq T_{1,0} · R_{45} $$
**Note that matrices are applied right to left**
$$ T_{(1,0)} · R_{45}
\begin{bmatrix} 
x \\ 
y \\
1 \\
\end{bmatrix}
=
\begin{bmatrix} 
1 & 0 & 1 \\ 
0 & 1 & 0 \\
0 & 0 & 1 \\
\end{bmatrix}
\begin{bmatrix} 
\cos(45) & -\sin(45) & 0 \\ 
\sin(45) & \cos(45) & 0 \\
0 & 0 & 1 \\
\end{bmatrix}
\begin{bmatrix} 
x \\ 
y \\
1 \\
\end{bmatrix}
$$
### Decomposing Complex Transforms
How to rotate around a given point c?
```
1. Translate center to origin
2. Rotate
3. Translate back
```
$$
T_{(c)} · R_{(\alpha)} · T_{(-c)}
$$


### tips
1. <font color='red'>What`s the order?</font> Linear Transform first or Translation first?
```
jump to: Affine Transformations
Transform first, then Translation. 
```
