using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Drawing.Imaging; // for ImageFormat
using System.IO;//輸入讀取
using System.Globalization;

namespace WindowsApplication1
{
    public partial class Form1 : Form
    {
        int[,,] RGBdata;//原始的陣列
        int[,,] lastRGB;
        int[,,] newRGB;//各種更新後的
        static Image image;
        Image newImage;
        String Filename;//檔案名稱

        public Form1()
        {
            InitializeComponent();
        }
        // Load 按鈕事件處理函式 
        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.Cancel)
            {
                RGBExtraction RGBExtraction = new RGBExtraction();// 建立物件
                Filename = openFileDialog1.FileName; //檔案名稱
                LoadImage(Filename);
                RGBdata = RGBExtraction.getRGBData(image);//取得RGB
                newRGB = RGBExtraction.getRGBData(image);//取得RGB
                lastRGB = RGBExtraction.getRGBData(image);//取得RGB
                ImageForm MyImage = new ImageForm(image, "Input File"); // 建立秀圖物件
                MyImage.Show();// 顯示秀圖照片 
            }

        }

        // RGB Extraction & transformation 按鈕事件處理函式
        private void button2_Click(object sender, EventArgs e)
        {
            RGBExtraction RGBExtraction = new RGBExtraction();// 建立物件
            //R
            newRGB = RGBExtraction.doRGray(RGBdata, 0);
            newImage = RGB2Image(newRGB);
            ImageForm MyImageR = new ImageForm(newImage, "R picture (RGB Extraction & transformation)"); // 建立秀圖物件
            MyImageR.Show();// 顯示秀圖照片 

            //G
            newRGB = RGBExtraction.doRGray(RGBdata, 1);
            newImage = RGB2Image(newRGB);
            ImageForm MyImageG = new ImageForm(newImage, "G picture (RGB Extraction & transformation)"); // 建立秀圖物件
            MyImageG.Show();// 顯示秀圖照片

            //B
            newRGB = RGBExtraction.doRGray(RGBdata, 2);
            newImage = RGB2Image(newRGB);
            ImageForm MyImageB = new ImageForm(newImage, "B picture (RGB Extraction & transformation)"); // 建立秀圖物件
            MyImageB.Show();// 顯示秀圖照片
        }

        //Smooth filter
        private void button4_Click(object sender, EventArgs e)
        {
            lastRGB = newRGB;

            SmoothFilter SmoothFilter = new SmoothFilter();
            newRGB = SmoothFilter.MeanSmooth(lastRGB);
            newImage = RGB2Image(newRGB);
            ImageForm MyImageA = new ImageForm(newImage, "Mean (Smooth filter)"); // 建立秀圖物件
            MyImageA.Show();// 顯示秀圖照片

            newRGB = SmoothFilter.MedianSmooth(lastRGB);
            newImage = RGB2Image(newRGB);
            ImageForm MyImageB = new ImageForm(newImage, "Median (Smooth filter)"); // 建立秀圖物件
            MyImageB.Show();// 顯示秀圖照片
        }
        
        //show 壓縮率&失真率
        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        static public void LoadImage(String Filename)
        {   //載入檔案
            image = Image.FromFile(Filename);

        }

        static public Image RGB2Image(int[,,] data)
        {
            // Step 1: 建立 Bitmap 元件
            Bitmap bimage = new Bitmap(image);
            int Height = bimage.Height;
            int Width = bimage.Width;

            // Step 2: 設定像點資料
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    bimage.SetPixel(x, y, Color.FromArgb(data[0, y, x], data[1, y, x], data[2, y, x]));
                }
            }
            // Step 3: 更新顯示影像 
            return bimage;
        }

        // 建立一個專門秀圖的 Form 類別
        class ImageForm : Form
        {
            Image image; // 建構子 
            public ImageForm(Image image, String text)
            {
                this.image = image;
                this.Text = text;
                //調整視窗大小
                this.Height = image.Height;
                this.Width = image.Width;

                InitializeMyScrollBar();

            }
            //ScrollBar視窗滾動
            private void InitializeMyScrollBar()
            {
                VScrollBar vScrollBar1 = new VScrollBar();
                HScrollBar hScrollBar1 = new HScrollBar();
                vScrollBar1.Dock = DockStyle.Right;
                hScrollBar1.Dock = DockStyle.Bottom;
                Controls.Add(vScrollBar1);
                Controls.Add(hScrollBar1);

            }
            //顯示圖片
            protected override void OnPaint(PaintEventArgs e)
            {
                e.Graphics.DrawImage(image, 0, 0, image.Width, image.Height);
            }
        }

        class RGBExtraction : Form
        {
            public RGBExtraction()//
            {

            }

            public int[,,] getRGBData(Image image)
            {
                Bitmap bimage;

                // Step 1: 利用 Bitmap 將 image 包起來
                bimage = new Bitmap(image);
                Height = bimage.Height;
                Width = bimage.Width;
                //初始化陣列
                int[,,] rgbData = new int[3, Height, Width];

                // Step 2: 取得像點顏色資訊
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        Color color = bimage.GetPixel(x, y);
                        rgbData[0, y, x] = color.R;
                        rgbData[1, y, x] = color.G;
                        rgbData[2, y, x] = color.B;
                    }
                }
                return rgbData;
            }

            public int[,,] doRGray(int[,,] rgbData, int RGB)
            {
                int[,,] result = new int[rgbData.GetLength(0), rgbData.GetLength(1), rgbData.GetLength(2)];

                // Step 2: 設定像點資料
                for (int y = 0; y < rgbData.GetLength(1); y++)
                {
                    for (int x = 0; x < rgbData.GetLength(2); x++)
                    {
                        int gray = rgbData[RGB, y, x];
                        result[0, y, x] = gray;
                        result[1, y, x] = gray;
                        result[2, y, x] = gray;
                    }
                }
                // Step 3: 更新顯示影像 
                return result;
            }
           
        }

        class SmoothFilter:Form
        {
            public SmoothFilter()//
            {
            }

            public int[,,] MeanSmooth(int[,,] rgbData)
            {
                int[,,] result = new int[rgbData.GetLength(0), rgbData.GetLength(1), rgbData.GetLength(2)];

                // Step 2: 設定像點資料
                for (int y = 0; y < rgbData.GetLength(1); y+=3)
                {
                    for (int x = 0; x < rgbData.GetLength(2); x+=3)
                    {
                        for(int i=0; i< rgbData.GetLength(0); i++)
                        {
                            int total = 0;
                            int count = 0;
                            for (int ii = 0; ii < 3; ii++)
                            {
                                if ((y + ii) >= rgbData.GetLength(1))
                                    continue;
                                for (int jj = 0; jj < 3; jj++)
                                {
                                    if ((x + jj) >= rgbData.GetLength(2))
                                        continue;
                                    total += rgbData[i, y + ii, x + jj];
                                    count++;
                                }
                            }

                            for (int ii = 0; ii < 3; ii++)
                            {
                                if ((y + ii) >= rgbData.GetLength(1))
                                    continue;
                                for (int jj = 0; jj < 3; jj++)
                                {
                                    if ((x + jj) >= rgbData.GetLength(2))
                                        continue;
                                    result[i, y + ii, x + jj] = total / count;
                                }
                            }
                        }
                    }
                }
                // Step 3: 更新顯示影像 
                return result;
            }

            public int[,,] MedianSmooth(int[,,] rgbData)
            {
                int[,,] result = new int[rgbData.GetLength(0), rgbData.GetLength(1), rgbData.GetLength(2)];

                // Step 2: 設定像點資料
                for (int y = 0; y < rgbData.GetLength(1); y += 3)
                {
                    for (int x = 0; x < rgbData.GetLength(2); x += 3)
                    {
                        for (int i = 0; i < rgbData.GetLength(0); i++)
                        {
                            List<int> tmpLi = new List<int>();
                            int media = 0;
                            for (int ii = 0; ii < 3; ii++)
                            {
                                if ((y + ii) >= rgbData.GetLength(1))
                                    continue;
                                for (int jj = 0; jj < 3; jj++)
                                {
                                    if ((x + jj) >= rgbData.GetLength(2))
                                        continue;
                                    tmpLi.Add(rgbData[i, y + ii, x + jj]);
                                }
                            }

                            int[] total = tmpLi.ToArray();

                            //为了不修改arr值，对数组的计算和修改在tempArr数组中进行
                            int [] tempArr = new int[total.Length];
                            total.CopyTo(tempArr, 0);

                            //对数组进行排序
                            int temp;
                            for (int ii = 0; ii < tempArr.Length; ii++)
                            {
                                for (int j = ii; j < tempArr.Length; j++)
                                {
                                    if (tempArr[ii] > tempArr[j])
                                    {
                                        temp = tempArr[ii];
                                        tempArr[ii] = tempArr[j];
                                        tempArr[j] = temp;
                                    }
                                }
                            }

                            //针对数组元素的奇偶分类讨论
                            if(tempArr.Length == 1)
                            {
                                media = tempArr[0];
                            }
                            else if (tempArr.Length % 2 != 0)
                            {
                                media = tempArr[total.Length / 2 + 1];
                            }
                            else
                            {
                                media = (tempArr[tempArr.Length / 2] + tempArr[(tempArr.Length / 2) + 1]) / 2;
                            }

                            for (int ii = 0; ii < 3; ii++)
                            {
                                if ((y + ii) >= rgbData.GetLength(1))
                                    continue;
                                for (int jj = 0; jj < 3; jj++)
                                {
                                    if ((x + jj) >= rgbData.GetLength(2))
                                        continue;
                                    result[i, y + ii, x + jj] = media;
                                }
                            }
                        }
                    }
                }
                // Step 3: 更新顯示影像 
                return result;
            }
        }

        class histogramEqualization
        {
            public histogramEqualization() { }

           

        }

        class Thresholding:Form
        {
            public Thresholding()
            {

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
