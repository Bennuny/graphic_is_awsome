#include<cmath>
#include"eigen3/Eigen/Core"
#include"eigen3/Eigen/Dense"
#include<iostream>

// 给定一个点 P=(2,1), 将该点绕原点先逆时针旋转 45◦，再平移 (1,2), 计算出 
// 变换后点的坐标（要求用齐次坐标进行计算
static void homework0(Eigen::Vector3f v);

void homework0(Eigen::Vector3f v)
{   
    // Transform
    // 1 0 1   x   x+1
    // 0 1 2 * y = y+2
    // 0 0 1   1   1

    // Rotate
    // cos(a)   -sin(a)     0     x
    // sin(a)   cos(a)      0   * y
    // 0        0           1     z

    // 1 0 1   x   x+1
    // 0 1 2 * y = y+2
    // 0 0 1   1   1

    Eigen::Matrix3f translate, rotate;

    translate << 1.0, 0, 1.0, 0, 1.0, 2.0, 0, 0, 1.0;

    rotate << std::cos(45.0/180.0*acos(-1)), -std::sin(45.0/180.0*acos(-1)), 0, std::sin(45.0/180.0*acos(-1)), std::cos(45.0/180.0*acos(-1)), 0, 0, 0, 1;

    Eigen::Matrix3f trans = translate * rotate;

    Eigen::Vector3f result = trans * v;

    std::cout << "homework0" << std::endl;
    std::cout << "rotate" << std::endl;
    std::cout << rotate << std::endl;

    std::cout << "translate" << std::endl;
    std::cout << translate << std::endl;

    std::cout << "trans" << std::endl;
    std::cout << trans << std::endl;

    std::cout << "result" << std::endl;
    std::cout << result << std::endl;

}

int main(){

    // Basic Example of cpp
    std::cout << "Example of cpp \n";
    float a = 1.0, b = 2.0;
    std::cout << a << std::endl;
    std::cout << a/b << std::endl;
    std::cout << std::sqrt(b) << std::endl;
    std::cout << std::acos(-1) << std::endl;
    std::cout << std::sin(30.0/180.0*acos(-1)) << std::endl;

    // Example of vector
    std::cout << "Example of vector \n";
    // vector definition
    Eigen::Vector3f v(1.0f,2.0f,3.0f);
    Eigen::Vector3f w(1.0f,0.0f,0.0f);
    // vector output
    std::cout << "Example of output \n";
    std::cout << v << std::endl;
    // vector add
    std::cout << "Example of add \n";
    std::cout << v + w << std::endl;
    // vector scalar multiply
    std::cout << "Example of scalar multiply \n";
    std::cout << v * 3.0f << std::endl;
    std::cout << 2.0f * v << std::endl;

    // Example of matrix
    std::cout << "Example of matrix \n";
    // matrix definition
    Eigen::Matrix3f i,j;
    i << 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0;
    j << 2.0, 3.0, 1.0, 4.0, 6.0, 5.0, 9.0, 7.0, 8.0;
    // matrix output
    std::cout << "Example of output \n";
    std::cout << i << std::endl;
    // matrix add i + j
    // matrix scalar multiply i * 2.0
    // matrix multiply i * j
    // matrix multiply vector i * v

    std::cout << "Example of Matrix Add" << std::endl;
    std::cout << i + j << std::endl;
    // 3 5 4 
    // 8 11 11
    // 16 15 17

    std::cout << "Example of Matrix scalar multiply i * 2.0" << std::endl;
    std::cout << i * 2.0 << std::endl;
    // 2,4,6,8,10,12,14,16,18

    std::cout << "Example of Matrix multiply i * j" << std::endl;
    std::cout << i * j << std::endl;
    // 1 2 3   2 3 1   37  36  35
    // 4 5 6 * 4 6 5 = 82  84  77
    // 7 8 9   9 7 8   127 132 119

    std::cout << "Example of Matrix multiply vector i * v" << std::endl;
    std::cout << i * v << std::endl;
    // 1 2 3   1   14
    // 4 5 6 * 2 = 32 
    // 7 8 9   3   50

    // homework0 (2, 1, 1)
    Eigen::Vector3f vh(2.0f,1.0f,1.0f);
    homework0(vh);

    return 0;
}