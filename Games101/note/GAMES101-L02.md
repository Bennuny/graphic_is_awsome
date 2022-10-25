向量

· 转置(Cartesian Coordinates)  
· Dot product  

$$ \vec{a} \cdot \vec{b}=||\vec{a}||||\vec{b}||\cos\theta $$
$$ \vec{a} \cdot \vec{b}=
\begin{pmatrix} 
x_a \\ 
y_a \\
\end{pmatrix}
·
\begin{pmatrix} 
x_b \\ 
y_b \\
\end{pmatrix}
=
x_ax_b+y_ay_b
$$

```
Find angle between two vector(e.g. cosine of angle between light source and surface)

Findind projection of one vector on another

Measure how close two directions are

Decompose a vector

Determine forward / backward
```

· Cross product
$$ \vec{a} \times \vec{b} = ||\vec{a}||||\vec{b}||\sin\theta $$
$$ \vec{a} \times \vec{b} = -\vec{a} \times \vec{b} $$
$$ \vec{a} \times \vec{b} = 
\begin{pmatrix} 
x_a \\ 
y_a \\
z_a \\
\end{pmatrix}
\times
\begin{pmatrix}
x_b \\
y_b \\
z_b \\
\end{pmatrix}
=
\begin{pmatrix}
y_az_b - y_bz_a \\
z_ay_b - x_az_b \\
x_ay_b - y_ax_b \\
\end{pmatrix}
$$ 
```
右手螺旋定则：四指(a旋转到b方向)

Determine left / right

Determine inside / outside

```

· Matrix
· Matrix-Matrix Multiplication
· Transpose of a Matrix: Switch rows and columns (ij -> ji)
$$ {AB}^T = B^TA^T $$
· Identify Matrix and Inverses
$$ {I}_{3*3} = 
\begin{pmatrix}
1 & 0 & 0 \\
0 & 1 & 0 \\
0 & 0 & 1 \\
\end{pmatrix}
$$
```

Element(i, j) in the product is the dot product of row i from A and column j from B

```


· **Vector multiplication in Matrix form**  
· Dot product?
$$ \vec{a} · \vec{b} = \vec{a}^T \vec{b} =
\begin{pmatrix}
x_a & y_a & z_a
\end{pmatrix}
\begin{pmatrix}
x_b \\
y_b \\
z_b \\
\end{pmatrix}
=
\begin{pmatrix}
x_a \times x_b + y_a \times y_b + z_a \times z_b
\end{pmatrix}
$$
· Cross product?
$$ \vec{a} \times \vec{b} =
A^*b = 
\begin{pmatrix}
0 && -z_a && y_a \\
z_a && 0 && -x_a \\
-y_a && x_a && 0 \\
\end{pmatrix}
\begin{pmatrix}
x_b \\
y_b \\
z_b \\
\end{pmatrix}
$$


***problem.***
1. dual matrix of vector a?

***推荐学习资源.***
1. 《线性代数及其应用》
2. Markdown公式输入：https://www.zybuluo.com/codeep/note/163962
