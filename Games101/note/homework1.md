### Projection


$$
M_{proj} = M_{orth} · M_{{proj}-{orth}}
$$


1.
$$
y^· = y·n/f
$$
$$
x^· = x·n/f
$$

2. 
$$
M_{{proj}-{orth}} 
\begin{pmatrix}
x \\
y \\
z \\
1 \\
\end{pmatrix}
=>
\begin{pmatrix}
nx \\
ny \\
unkonw \\
z \\
\end{pmatrix}
$$

- Observation: the third row is responsible for z`
    - Any point on the near plane will not change
    - Any point`s on the far plane will not change

$$
\begin{pmatrix}
x \\
y \\
n \\
1 \\
\end{pmatrix}
=>
\begin{pmatrix}
nx \\
ny \\
n^2 \\
n \\
\end{pmatrix}
$$

**与x、y无关**
$$
\begin{pmatrix}
A & B & C & D
\end{pmatrix}
\begin{pmatrix}
x \\
y \\
n \\
1 \\
\end{pmatrix}
\begin{pmatrix}
nx \\
ny \\
n^2 \\
n \\
\end{pmatrix}
$$

$$
M_{{proj}-{orth}} =
\begin{pmatrix}
n & 0 & 0 & 0 \\
0 & n & 0 & 0 \\
A & B & C & D \\
0 & 0 & 1 & 0 \\
\end{pmatrix}
$$

$$
\begin{pmatrix}
A \\
B \\
C \\
D \\
\end{pmatrix}
=>
\begin{pmatrix}
0 \\
0 \\
A \\
B \\
\end{pmatrix}
=>
\begin{pmatrix}
An+B = n^2 \\
Af+B = f^2 \\
\end{pmatrix}
=>
\begin{pmatrix}
A = n+f \\
B = -fn \\
\end{pmatrix}
$$

### Result
$$
M_{{proj}-{orth}} =
\begin{pmatrix}
n & 0 & 0 & 0 \\
0 & n & 0 & 0 \\
0 & 0 & n+f & -nf \\
0 & 0 & 1 & 0 \\
\end{pmatrix}
$$

$$
M_{{orth}} =
\begin{pmatrix}
\frac{2}{r-l} & 0 & 0 & \frac{l+r}{2} \\
0 & \frac{2}{b-t} & 0 & \frac{b+t}{2} \\
0 & 0 & \frac{2}{f-n} & \frac{n+f}{2} \\
0 & 0 & 0 & 1 \\
\end{pmatrix}
$$

$$
M_{proj} = M_{{proj}-{orth}} M_{orth}
$$


### 绕任意轴旋转（罗德里戈斯）
#### 向量Review
向量点乘和叉乘的区别：向量点乘结果是标量，是两个向量的一个方向的累计结果，结果只保留大小属性，抹去方向属性，相当于是降维；
向量叉乘，是这两个向量平面上，垂直生成新的向量，大小是两个向量构成四边形的乘积。相当于升维。
点乘的结果表示向量A在向量B方向上的投影与向量B模的的乘积.
这是运算所需要，向量加和减都是在同一纬度空间操作的，如果想要实现纬度的变化就要在向量的乘法做出定义。

$$
\vec{a} \cdot\vec{b} = \lvert\vec{a}\lvert\lvert\vec{b}\lvert\cos\theta
$$

$$
\vec{a} \times \vec{b} = \lvert\vec{a}\lvert\lvert\vec{b}\lvert\sin\theta\vec{n}
$$

<image width="200" height="200" src="https://pic3.zhimg.com/80/v2-215c9038f144da8e723731082c043056_1440w.webp"/>


v为待旋转向量，k为旋转轴。

$$
v_{rot} = \cos\theta v + (1-\cos\theta)(v \cdot k)k + \sin\theta k \times v
$$
其中, \theta 为旋转角度，v为待旋转向量，k为旋转向量（单位向量），v_rot为旋转后的向量  

[罗德里格公式推导及理解](https://zhuanlan.zhihu.com/p/390522246)