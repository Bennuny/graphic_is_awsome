### 作业3

### 1. [todo]insideTriangle。
```
    贴一下HW3中的实现

static bool insideTriangle(int x, int y, const Vector4f* _v){
    Vector3f v[3];
    for(int i=0;i<3;i++)
        v[i] = {_v[i].x(),_v[i].y(), 1.0};
    
    Vector3f f0,f1,f2;
    f0 = v[1].cross(v[0]);
    f1 = v[2].cross(v[1]);
    f2 = v[0].cross(v[2]);
    
    Vector3f p(x,y,1.);
    
    if((p.dot(f0)*f0.dot(v[2])>0) && (p.dot(f1)*f1.dot(v[0])>0) && (p.dot(f2)*f2.dot(v[1])>0))
        return true;
    
    return false;
}

```

1. get_view_matrix(Vector3f eye_pos). 将观察点，摄像机位置移到原点；
2. get_model_matrix(float angle). 模型变换，模型进行，缩放，旋转（罗德里戈斯，绕任意轴旋转公式），平移；
3. get_projection_matrix(float eye_fov, aspect_ratio, zNear, zFar). 根据zNear, zFar 计算挤压矩阵Mperp-orth, 计算正交投影，将坐标映射到[-1, 1]范围内；

- vertex_shader
- normal_fragment_shader: 法线-fragment

- texture_fragment_shader

- phong_fragment_shader

- displacement_fragment_shader

- bump_fragment_shader

- reflect


### key function
- insideTriangle
- rasterizer(Bresenham's line drawing algorithm)
- computeBarycentric2D
- interpolate 线性插值