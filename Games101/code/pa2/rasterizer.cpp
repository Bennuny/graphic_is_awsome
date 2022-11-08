// clang-format off
//
// Created by goksu on 4/6/19.
//

#include <algorithm>
#include <vector>
#include "rasterizer.hpp"
#include <opencv2/opencv.hpp>
#include <math.h>


rst::pos_buf_id rst::rasterizer::load_positions(const std::vector<Eigen::Vector3f> &positions)
{
    auto id = get_next_id();
    pos_buf.emplace(id, positions);

    return {id};
}

rst::ind_buf_id rst::rasterizer::load_indices(const std::vector<Eigen::Vector3i> &indices)
{
    auto id = get_next_id();
    ind_buf.emplace(id, indices);

    return {id};
}

rst::col_buf_id rst::rasterizer::load_colors(const std::vector<Eigen::Vector3f> &cols)
{
    auto id = get_next_id();
    col_buf.emplace(id, cols);

    return {id};
}

auto to_vec4(const Eigen::Vector3f& v3, float w = 1.0f)
{
    return Vector4f(v3.x(), v3.y(), v3.z(), w);
}


static bool insideTriangle(float x, float y, const Vector3f* _v)
{   
    // TODO : Implement this function to check if the point (x, y) is inside the triangle represented by _v[0], _v[1], _v[2]

    bool in = false;

    Eigen::Vector3f p(x, y, 1.0f);

    Eigen::Vector3f AB = _v[1] - _v[0];
    Eigen::Vector3f BC = _v[2] - _v[1];
    Eigen::Vector3f CA = _v[0] - _v[2];

    Eigen::Vector3f AP = p - _v[0];
    Eigen::Vector3f BP = p - _v[1];
    Eigen::Vector3f CP = p - _v[2];

    float za = AB.cross(AP).z();
    float zb = BC.cross(BP).z();
    float zc = CA.cross(CP).z();

    in = (za > 0 && zb > 0 && zc > 0) || (za < 0 && zb < 0 && zc < 0);

    return in;
}

static std::tuple<float, float, float> computeBarycentric2D(float x, float y, const Vector3f* v)
{
    float c1 = (x*(v[1].y() - v[2].y()) + (v[2].x() - v[1].x())*y + v[1].x()*v[2].y() - v[2].x()*v[1].y()) / (v[0].x()*(v[1].y() - v[2].y()) + (v[2].x() - v[1].x())*v[0].y() + v[1].x()*v[2].y() - v[2].x()*v[1].y());
    float c2 = (x*(v[2].y() - v[0].y()) + (v[0].x() - v[2].x())*y + v[2].x()*v[0].y() - v[0].x()*v[2].y()) / (v[1].x()*(v[2].y() - v[0].y()) + (v[0].x() - v[2].x())*v[1].y() + v[2].x()*v[0].y() - v[0].x()*v[2].y());
    float c3 = (x*(v[0].y() - v[1].y()) + (v[1].x() - v[0].x())*y + v[0].x()*v[1].y() - v[1].x()*v[0].y()) / (v[2].x()*(v[0].y() - v[1].y()) + (v[1].x() - v[0].x())*v[2].y() + v[0].x()*v[1].y() - v[1].x()*v[0].y());
    return {c1,c2,c3};
}

void rst::rasterizer::draw(pos_buf_id pos_buffer, ind_buf_id ind_buffer, col_buf_id col_buffer, Primitive type)
{
    auto& buf = pos_buf[pos_buffer.pos_id];
    auto& ind = ind_buf[ind_buffer.ind_id];
    auto& col = col_buf[col_buffer.col_id];

    float f1 = (50 - 0.1) / 2.0;
    float f2 = (50 + 0.1) / 2.0;

    Eigen::Matrix4f mvp = projection * view * model;
    for (auto& i : ind)
    {
        Triangle t;
        Eigen::Vector4f v[] = {
                mvp * to_vec4(buf[i[0]], 1.0f),
                mvp * to_vec4(buf[i[1]], 1.0f),
                mvp * to_vec4(buf[i[2]], 1.0f)
        };
        //Homogeneous division
        for (auto& vec : v) {
            vec /= vec.w();
        }
        //Viewport transformation
        for (auto & vert : v)
        {
            vert.x() = 0.5*width*(vert.x()+1.0);
            vert.y() = 0.5*height*(vert.y()+1.0);
            vert.z() = vert.z() * f1 + f2;
        }

        for (int i = 0; i < 3; ++i)
        {
            t.setVertex(i, v[i].head<3>());
            t.setVertex(i, v[i].head<3>());
            t.setVertex(i, v[i].head<3>());
        }

        auto col_x = col[i[0]];
        auto col_y = col[i[1]];
        auto col_z = col[i[2]];

        t.setColor(0, col_x[0], col_x[1], col_x[2]);
        t.setColor(1, col_y[0], col_y[1], col_y[2]);
        t.setColor(2, col_z[0], col_z[1], col_z[2]);

        rasterize_triangle(t);
    }
}

//Screen space rasterization
void rst::rasterizer::rasterize_triangle(const Triangle& t) {
    auto v = t.toVector4();
    
    std::cout << "rasterize_triangle begin: " << std::endl;
    // std::cout << "0:" << v[0] << "\n1:" << v[1] << "\n2:" << v[2] << "\n3:" << v[3] << std::endl;

    // bounding box
    float minx = v[0][0], maxx = v[0][0];
    float miny = v[0][1], maxy = v[0][1];
    for (size_t i = 1; i < 3; i++)
    {
        minx = std::min(minx, v[i][0]);
        maxx = std::max(maxx, v[i][0]);
        miny = std::min(miny, v[i][1]);
        maxy = std::max(maxy, v[i][1]);
    }
    // std::cout << minx << ", " << maxx << ", "<< miny << "," << maxy << std::endl;

    // TODO : Find out the bounding box of current triangle.
    // iterate through the pixel and find if the current pixel is inside the triangle

    // for (int px = minx; px <= static_cast<int>(maxx); px += 1.f)
    // {
    //     for (int py = miny; py <= static_cast<int>(maxy); py += 1.f)
    //     {
    //         bool inTriangle = insideTriangle(static_cast<float>(px+0.5), static_cast<float>(py+0.5), t.v);

    //         if (inTriangle)
    //         {
    //             auto [alpha, beta, gamma] = computeBarycentric2D(px, py, t.v);
    //             float w_reciprocal = 1.0/(alpha / v[0].w() + beta / v[1].w() + gamma / v[2].w());
    //             float z_interpolated = alpha * v[0].z() / v[0].w() + beta * v[1].z() / v[1].w() + gamma * v[2].z() / v[2].w();
    //             z_interpolated *= w_reciprocal;
    //             // 越大离视点越远

    //             float idx = get_index(px, py);
    //             if (z_interpolated < depth_buf[idx]) 
    //             {
    //                 depth_buf[idx] = z_interpolated;
    //                 set_pixel(Eigen::Vector3f(static_cast<float>(px), static_cast<float>(py), z_interpolated), t.getColor());
    //             }
    //         }
    //     }
    // }

    for (int px = minx; px <= static_cast<int>(maxx); px += 1.f)
    {
        for (int py = miny; py <= static_cast<int>(maxy); py += 1.f)
        {
            bool isSetPixel = false;
            for (float i = 0; i < 1.f; i += 1.f/msaa_w)
            {
                for (float j = 0; j < 1.f; j += 1.f/msaa_h)
                {
                    Vector3f subP(px+i, py+j, 0.f);

                    if (!insideTriangle(subP.x(), subP.y(), t.v))                    
                    {
                        continue;
                    }
                    
                    auto [alpha, beta, gamma] = computeBarycentric2D(px, py, t.v);
                    float w_reciprocal = 1.0/(alpha / v[0].w() + beta / v[1].w() + gamma / v[2].w());
                    float z_interpolated = alpha * v[0].z() / v[0].w() + beta * v[1].z() / v[1].w() + gamma * v[2].z() / v[2].w();
                    z_interpolated *= w_reciprocal;
                    // 越大离视点越远

                    float idx = get_msaa_index(px + i, py + j);
                    if (z_interpolated < depth_buf[idx]) 
                    {
                        depth_buf[idx] = z_interpolated;
                        color_buf[idx] = t.getColor();
                        // set_pixel(Eigen::Vector3f(static_cast<float>(px), static_cast<float>(py), z_interpolated), t.getColor());
                        isSetPixel = true;
                    }
                }
            }

            if (!isSetPixel)
            {
                continue;
            }

            int buf_idx = get_index(px, py);
            Vector3f comb_color = Vector3f::Zero();

            for (float i = 0; i < 1; i += 1.f/msaa_w)
            {
                for (float j = 0; j < 1; j += 1.f/msaa_h)
                {
                    int msaa_idx = get_msaa_index(px+i, py+j);
                    comb_color += (1.f / (msaa_w * msaa_h)) * color_buf[msaa_idx];

                    std::cout << "comb_color: " << comb_color << std::endl;
                }
            }
            set_pixel(Vector3f(px, py, 0), comb_color);
        }
    }


    // If so, use the following code to get the interpolated z value.
    //auto[alpha, beta, gamma] = computeBarycentric2D(x, y, t.v);
    //float w_reciprocal = 1.0/(alpha / v[0].w() + beta / v[1].w() + gamma / v[2].w());
    //float z_interpolated = alpha * v[0].z() / v[0].w() + beta * v[1].z() / v[1].w() + gamma * v[2].z() / v[2].w();
    //z_interpolated *= w_reciprocal;

    // TODO : set the current pixel (use the set_pixel function) to the color of the triangle (use getColor function) if it should be painted.

}

void rst::rasterizer::set_model(const Eigen::Matrix4f& m)
{
    model = m;
}

void rst::rasterizer::set_view(const Eigen::Matrix4f& v)
{
    view = v;
}

void rst::rasterizer::set_projection(const Eigen::Matrix4f& p)
{
    projection = p;
}

void rst::rasterizer::clear(rst::Buffers buff)
{
    if ((buff & rst::Buffers::Color) == rst::Buffers::Color)
    {
        std::fill(frame_buf.begin(), frame_buf.end(), Eigen::Vector3f{0, 0, 0});

        std::fill(color_buf.begin(), color_buf.end(), Eigen::Vector3f{0, 0, 0});
    }
    if ((buff & rst::Buffers::Depth) == rst::Buffers::Depth)
    {
        std::fill(depth_buf.begin(), depth_buf.end(), std::numeric_limits<float>::infinity());
    }
}

rst::rasterizer::rasterizer(int w, int h) : width(w), height(h)
{
    msaa_w = 2;
    msaa_h = 2;

    frame_buf.resize(w * h);
    depth_buf.resize(w * h * msaa_w * msaa_h);
    color_buf.resize(w * h * msaa_w * msaa_h);
}

int rst::rasterizer::get_index(int x, int y)
{
    return (height-1-y)*width + x;
}

int rst::rasterizer::get_msaa_index(float x, float y)
{
    x = x * msaa_w;
    y = y * msaa_h;

    return (height*msaa_h-1-y)*width*msaa_w + x;
}

void rst::rasterizer::set_pixel(const Eigen::Vector3f& point, const Eigen::Vector3f& color)
{
    //old index: auto ind = point.y() + point.x() * width;
    auto ind = (height-1-point.y())*width + point.x();
    frame_buf[ind] = color;

}

// clang-format on