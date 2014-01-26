﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace YouMaps
{
    class YMMatrix
    {
        public static Matrix Translate(this Matrix matrix, double offsetX, double offsetY)
        {
            matrix.OffsetX += offsetX;
            matrix.OffsetY += offsetY;
            return matrix;
        }

        public static Matrix Scale(this Matrix matrix, double scaleX, double scaleY)
        {
            return Multiply(matrix, new Matrix(scaleX, 0d, 0d, scaleY, od, od));
        }

        public static Matrix Rotate(this Matrix matrix, double angle)
        {
            angle = (angle % 360d) / 180 * Math.PI;
            var cos = Math.Cos(angle);
            var sin = Math.Sin(angle);
            return Multiply(matrix, new Matrix(cos, sin, -sin, cos, 0d, 0d));
        }

        public static Matrix RotateAt(this Matrix matrix, double angle, double centerX, double centerY)
        {
            angle = (angle % 360d) / 180d * Math.PI;
            var cos = Math.Cos(angle);
            var sin = Math.Sin(angle);
            var offsetX = centerX * (1d - cos) + centerY * sin;
            var offsetY = centerY * (1d - cos) - centerX * sin;
            return Multiply(matrix, new Matrix(cos, sin, -sin, cos, offsetX, offsetY));
        }

        public static Matrix Invert(this Matrix matrix)
        {
            var determinant = matrix.M11 * matrix.M22 - matrix.M12 * matrix.M21;
            return new Matrix(matrix.M22/determinant, -matrix.M12 /determinant,
                -matrix.M21 / determinant, matrix.M11 / determinant,
                (matrix.M21 * matrix.OffsetY - matrix.M22 * matrix.OffsetX) / determinant,
                (matrix.M12 * matrix.OffsetX - matrix.M11 * matrix.OffsetY) / determinant);
        }

        private static Matrix Multiply(Matrix matrix1, Matrix matrix2)
        {
             return new Matrix(
                matrix1.M11 * matrix2.M11 + matrix1.M12 * matrix2.M21,
                matrix1.M11 * matrix2.M12 + matrix1.M12 * matrix2.M22,
                matrix1.M21 * matrix2.M11 + matrix1.M22 * matrix2.M21,
                matrix1.M21 * matrix2.M12 + matrix1.M22 * matrix2.M22,
                (matrix2.M11 * matrix1.OffsetX + matrix2.M21 * matrix1.OffsetY) + matrix2.OffsetX,
                (matrix2.M12 * matrix1.OffsetX + matrix2.M22 * matrix1.OffsetY) + matrix2.OffsetY);
        }
    }
}
