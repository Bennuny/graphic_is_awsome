#include "Triangle.hpp"
#include "rasterizer.hpp"
#include <eigen3/Eigen/Eigen>
#include <iostream>
#include <opencv2/opencv.hpp>

constexpr double MY_PI = 3.1415926;

Eigen::Matrix4f get_view_matrix(Eigen::Vector3f eye_pos)
{
    Eigen::Matrix4f view = Eigen::Matrix4f::Identity();

    Eigen::Matrix4f translate;
    translate << 1, 0, 0, -eye_pos[0], 0, 1, 0, -eye_pos[1], 0, 0, 1,
        -eye_pos[2], 0, 0, 0, 1;

    view = translate * view;

    return view;
}

Eigen::Matrix4f get_model_matrix(float rotation_angle)
{
    Eigen::Matrix4f model = Eigen::Matrix4f::Identity();

    // TODO: Implement this function
    // Create the model matrix for rotating the triangle around the Z axis.
    // Then return it.

    // 1,0,z,1 -> cosA, sinA, z, 1
    // 0,1,z,1 -> -sinA, cosA, z, 1

    // 1 0 0 0      
    // 0 1 0 0 ->    
    // 0 0 1 0      
    // 0 0 0 1      

    float pi = std::acos(-1.0);

    model << std::cos(rotation_angle/180.0f*pi), std::sin(rotation_angle/180.0f*pi), 0, 0, -std::sin(rotation_angle/180.0f*pi), std::cos(rotation_angle/180.0f*pi), 0, 0, 0, 0, 1, 0, 0, 0, 0, 1;

    std::cout << "model_matrix rotate-z: " << rotation_angle << std::endl;
    std::cout << model << std::endl;

    return model;
}

// ex
Eigen::Matrix4f get_rotation(Vector3f axis, float angle)
{
    Eigen::Matrix4f model = Eigen::Matrix4f::Identity();

    // $$
    // v_{rot} = \cos\theta v + (1-\cos\theta)(v \cdot k)k + \sin\theta k \times v
    // $$
    // 其中, \theta 为旋转角度，v为待旋转向量，k为旋转向量（单位向量），v_rot为旋转后的向量  

    // TODO: Implement this function
    // Create the model matrix for rotating the triangle around the Z axis.
    // Then return it.

    // ！！！向量写成矩阵的形式

    // 罗德里格公式
    float pi = std::acos(-1);
    float cos_v = std::cos(angle/180.0*pi);
    float sin_v = std::sin(angle/180.0*pi);

    float nx = axis[0];
    float ny = axis[1];
    float nz = axis[2];

    Eigen::Matrix3f N;
    N << 0, -nz, ny, nz, 0, -nx, -ny, nx, 0;

    Eigen::Matrix3f I = Eigen::Matrix3f::Identity();

    Eigen::Matrix3f R = cos_v * I + (1-cos_v) * axis * axis.transpose() + sin_v * N;

    model << 
        R(0, 0), R(0, 1), R(0, 2), 0,
        R(1, 0), R(1, 1), R(1, 2), 0,
        R(2, 0), R(2, 1), R(2, 2), 0,
        0, 0, 0, 1.0;

    return model;
}

Eigen::Matrix4f get_projection_matrix(float eye_fov, float aspect_ratio,
                                      float zNear, float zFar)
{
    // Students will implement this function

    Eigen::Matrix4f projection = Eigen::Matrix4f::Identity();

    std::cout << "get_projection_matrix" << std::endl;

    // 1. Mprojection-orthgraphic
    Eigen::Matrix4f mpo = Eigen::Matrix4f::Identity();
    mpo << zNear, 0, 0, 0, 0, zNear, 0, 0, 0, 0, zNear + zFar, -zNear * zFar, 0, 0, 1, 0;

    std::cout << "projection 挤压" << std::endl;
    std::cout << mpo << std::endl;

    float pi = std::acos(-1.0);
    float tfov = std::tan(eye_fov/2.0/180.0*pi);
    // tfov = t / zNear;

    float top = -zNear * tfov;
    float bottom = -top;

    // aspect_ratio = width / height
    float width = aspect_ratio * (bottom - top);
    float l = -(width/2.0);
    float r = -l;

    std::cout << "top bottom left right near far" << std::endl;
    std::cout << top << bottom << l << r << zNear << zFar << std::endl;

    // 2. Orthgraphic-Transformation
    Eigen::Matrix4f otr = Eigen::Matrix4f::Identity();
    otr << 1, 0, 0, (r+l)/2.0, 0, 1, 0, (bottom+top)/2.0, 0, 0, 1, (zNear+zFar)/2.0, 0, 0, 0, 1;

    std::cout << "orthgraphic 平移" << std::endl;
    std::cout << otr << std::endl;


    // 3. Orthgrahpic-Scale
    Eigen::Matrix4f osc = Eigen::Matrix4f::Identity();
    osc << 2.0/(r-l), 0, 0, 0, 0, 2.0/(bottom-top), 0, 0, 0, 0, 2.0/(zFar-zNear), 0, 0, 0, 0, 1;

    std::cout << "orthgraphic 缩放" << std::endl;
    std::cout << osc << std::endl;

    projection = osc * otr * mpo;

    std::cout << "projection" << std::endl;
    std::cout << projection << std::endl;

    // TODO: Implement this function
    // Create the projection matrix for the given parameters.
    // Then return it.

    return projection;
}

int main(int argc, const char** argv)
{
    float angle = 0;
    bool command_line = false;
    std::string filename = "output.png";

    if (argc >= 3) {
        command_line = true;
        angle = std::stof(argv[2]); // -r by default
        if (argc == 4) {
            filename = std::string(argv[3]);
        }
        else
            return 0;
    }

    rst::rasterizer r(700, 700);

    Eigen::Vector3f eye_pos = {0, 0, 5};

    std::vector<Eigen::Vector3f> pos{{2, 0, -2}, {0, 2, -2}, {-2, 0, -2}};

    std::vector<Eigen::Vector3i> ind{{0, 1, 2}};

    auto pos_id = r.load_positions(pos);
    auto ind_id = r.load_indices(ind);

    int key = 0;
    int frame_count = 0;

    if (command_line) {
        r.clear(rst::Buffers::Color | rst::Buffers::Depth);

        r.set_model(get_model_matrix(angle));
        r.set_view(get_view_matrix(eye_pos));
        r.set_projection(get_projection_matrix(45, 1, 0.1, 50));

        r.draw(pos_id, ind_id, rst::Primitive::Triangle);
        cv::Mat image(700, 700, CV_32FC3, r.frame_buffer().data());
        image.convertTo(image, CV_8UC3, 1.0f);

        cv::imwrite(filename, image);

        return 0;
    }

    while (key != 27) {
        r.clear(rst::Buffers::Color | rst::Buffers::Depth);

        // r.set_model(get_model_matrix(angle));

        Eigen::Vector3f axis;
        axis << 0, 2.0, -2.0;
        r.set_model(get_rotation(axis, angle));
        
        r.set_view(get_view_matrix(eye_pos));
        r.set_projection(get_projection_matrix(45, 1, 0.1, 50));

        r.draw(pos_id, ind_id, rst::Primitive::Triangle);

        cv::Mat image(700, 700, CV_32FC3, r.frame_buffer().data());
        image.convertTo(image, CV_8UC3, 1.0f);
        cv::imshow("image", image);
        key = cv::waitKey(10);

        std::cout << "frame count: " << frame_count++ << '\n';

        if (key == 'a') {
            angle += 10;
        }
        else if (key == 'd') {
            angle -= 10;
        }
    }

    return 0;
}
