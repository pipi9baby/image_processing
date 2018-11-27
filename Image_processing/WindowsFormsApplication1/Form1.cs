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

            //mean
            newRGB = RGBExtraction.meanRGB(RGBdata);
            newImage = RGB2Image(newRGB);
            ImageForm MyImageD = new ImageForm(newImage, "mean picture (RGB Extraction & transformation)"); // 建立秀圖物件
            MyImageD.Show();// 顯示秀圖照片
        }

        //Smooth filter
        private void button3_Click(object sender, EventArgs e)
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

        //累積機率函數那個
        private void button4_Click(object sender, EventArgs e)
        {
            lastRGB = newRGB;

            histogramEqualization histogramEqualization = new histogramEqualization();
            newRGB = histogramEqualization.Convert2Histogram(lastRGB);
            newImage = RGB2Image(newRGB);
            ImageForm MyImageA = new ImageForm(newImage, "Histogram Equalization"); // 建立秀圖物件
            MyImageA.Show();// 顯示秀圖照片
        }

        //A user-definedthresholding
        private void button5_Click_1(object sender, EventArgs e)
        {
            lastRGB = newRGB;
            string strtxt = textBox1.Text;
            int th = Int32.Parse(strtxt);

            Thresholding Thresholding = new Thresholding();
            newRGB = Thresholding.OutputThresholding(lastRGB, th);

            newImage = RGB2Image(newRGB);
            ImageForm MyImageA = new ImageForm(newImage, "User-Definedthresholding"); // 建立秀圖物件
            MyImageA.Show();// 顯示秀圖照片
        }
        
        //Sobel edge detection 
        private void button6_Click(object sender, EventArgs e)
        {
            lastRGB = newRGB;
            string strtxt = textBox1.Text;
            int th = Int32.Parse(strtxt);

            SobelEdgeDetection SobelEdgeDetection = new SobelEdgeDetection();
            int[,,]GX = SobelEdgeDetection.CalGX(lastRGB);
            int[,,]GY = SobelEdgeDetection.CalGY(lastRGB);

            newRGB = SobelEdgeDetection.Sobel(lastRGB, th, GX);
            newImage = RGB2Image(newRGB);
            ImageForm MyImageA = new ImageForm(newImage, "Vertical (Sobel edge detection)"); // 建立秀圖物件
            MyImageA.Show();// 顯示秀圖照片

            newRGB = SobelEdgeDetection.Sobel(lastRGB, th, GY);
            newImage = RGB2Image(newRGB);
            ImageForm MyImageB = new ImageForm(newImage, "Horizontal (Sobel edge detection)"); // 建立秀圖物件
            MyImageB.Show();// 顯示秀圖照片

            newRGB = SobelEdgeDetection.Sobel(lastRGB, th, GX, GY);
            newImage = RGB2Image(newRGB);
            ImageForm MyImageC = new ImageForm(newImage, "Combine (Sobel edge detection)"); // 建立秀圖物件
            MyImageC.Show();// 顯示秀圖照片
        }

        //Binary image overlap
        private void button7_Click(object sender, EventArgs e)
        {
            lastRGB = newRGB;
            string strtxt = textBox1.Text;
            int th = Int32.Parse(strtxt);

            BinaryImageOverlap BinaryImageOverlap = new BinaryImageOverlap();
            newRGB = BinaryImageOverlap.CalOverlap(lastRGB, th);
            newImage = RGB2Image(newRGB);
            ImageForm MyImageA = new ImageForm(newImage, "Binary image overlap"); // 建立秀圖物件
            MyImageA.Show();// 顯示秀圖照片
        }

        //Connect Componet
        private void button8_Click(object sender, EventArgs e)
        {
            ConnectedComponent ConnectedComponent = new ConnectedComponent();
            int componetNum = ConnectedComponent.CountObject(RGBdata);
            label3.Text = componetNum.ToString();
            newImage = RGB2Image(RGBdata);
            ImageForm MyImageA = new ImageForm(newImage, "Connect Componet"); // 建立秀圖物件
            MyImageA.Show();// 顯示秀圖照片
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lastRGB = newRGB;


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
            
            public int[,,] meanRGB(int[,,] data)
            {
                int[,,] result = (int[,,])data.Clone();

                // Step 2: 設定像點資料
                for (int y = 0; y < data.GetLength(1); y++)
                {
                    for (int x = 0; x < data.GetLength(2); x++)
                    {
                        int gray = (data[0, y, x] + data[1, y, x] + data[2, y, x])/3;
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

           
            public int[,,] Convert2Histogram(int[,,] data){

                int[,,] result = new int[data.GetLength(0), data.GetLength(1), data.GetLength(2)];

                int[] crf = staticCrf(data);

                for (int i = 0; i < data.GetLength(1);i++){
                    for (int j = 0; j < data.GetLength(2);j++){
                        result[0, i, j] = crf[data[0, i, j]];
                        result[1, i, j] = crf[data[1, i, j]];
                        result[2, i, j] = crf[data[2, i, j]];
                    }
                }
                    

                return result;
            }

            private int[] staticCrf(int[,,] data){
                
                int[] tmpArr = new int[256];

                //重置為0
                for (int i=0; i < tmpArr.Length;i++){
                    tmpArr[i] = 0;
                }

                //計算每個色階出現的次數
                for (int i = 0; i < data.GetLength(1); i++)
                {
                    for (int j = 0; j < data.GetLength(2); j++){
                        tmpArr[data[0, i, j]] += 1;
                    }
                }
                int max = 0;
                int min = int.MaxValue;
                //找最大最小
                for(int i = 0; i < tmpArr.Length; i++)
                {
                    if (tmpArr[i] < min & tmpArr[i] != 0)
                        min = tmpArr[i];
                }

                //計算累積分佈函數
                int total = 0;
                for (int i = 0; i < tmpArr.Length;i++){
                    total += tmpArr[i];
                    tmpArr[i] = total;
                }

                for (int i = 0; i < tmpArr.Length; i++)
                {
                    if (tmpArr[i] == 0)
                        continue;
                    float tmp = (tmpArr[i] - min) / (float)(tmpArr[255] - min);
                    tmpArr[i] = (int)(tmp * 255);
                }

                return tmpArr;
            }
        }

        class Thresholding:Form
        {
            public Thresholding()
            {
            }

            public int[,,] OutputThresholding(int[,,] data, int door){

                int[,,] result = new int[data.GetLength(0), data.GetLength(1), data.GetLength(2)];

                for (int i = 0; i < data.GetLength(1); i++)
                {
                    for (int j = 0; j < data.GetLength(2); j++)
                    {
                        for (int ch = 0; ch < data.GetLength(0); ch++)
                        {
                            if (data[ch, i, j] >= door)
                                result[ch, i, j] = 255;
                            else
                                result[ch, i, j] = 0;
                        }
                    }
                }

                return result;
            }
        }

        class SobelEdgeDetection{
            public SobelEdgeDetection(){}

            /// <summary>
            /// 最終決定要保留原色還是變成白色
            /// </summary>
            /// <returns>The sobel.</returns>
            /// <param name="data">Data.</param>
            /// <param name="door">Door.</param>
            /// <param name="G">G.</param>
            public int[,,] Sobel(int[,,] data, int door, int[,,] G){
                int[,,] result = (int[,,])data.Clone();

                for (int ch = 0; ch < data.GetLength(0); ch++)
                {
                    for (int i = 1; i < data.GetLength(1)-1; i++)
                    {
                        for (int j = 1; j < data.GetLength(2)-1; j++)
                        {
                            if (G[ch, i - 1, j - 1] >= door)
                                result[ch, i, j] = 255;
                        }
                    }
                }

                return result;
            }

            /// <summary>
            /// Combine GX和GY
            /// </summary>
            /// <returns>The sobel.</returns>
            /// <param name="data">Data.</param>
            /// <param name="door">Door.</param>
            /// <param name="GX">Gx.</param>
            /// <param name="GY">Gy.</param>
            public int[,,] Sobel(int[,,] data, int door, int[,,] GX, int[,,] GY)
            {
                int[,,] result = (int[,,])data.Clone();

                for (int ch = 0; ch < data.GetLength(0); ch++)
                {
                    for (int i = 1; i < data.GetLength(1) - 1; i++)
                    {
                        for (int j = 1; j < data.GetLength(2) - 1; j++)
                        {
                            double G = Math.Pow(Math.Pow(GX[ch, i - 1, j - 1], 2) + Math.Pow(GY[ch, i - 1, j - 1], 2), 0.5);

                            if (G >= door)
                                result[ch, i, j] = 255;
                        }
                    }
                }

                return result;
            }

            /// <summary>
            /// 計算GX矩陣
            /// </summary>
            /// <returns>The gx.</returns>
            /// <param name="data">Data.</param>
            public int[,,] CalGX(int[,,] data){

                int[,,] result = new int[data.GetLength(0), data.GetLength(1)-2, data.GetLength(2)-2];

                for (int i = 1; i < data.GetLength(1)-1;i++){
                    for (int j = 1; j < data.GetLength(2)-1;j++){
                        for (int ch = 0; ch < data.GetLength(0);ch++){

                            result[ch, i - 1, j - 1] = data[ch, i - 1, j - 1] - data[ch, i - 1, j + 1] + 2 * data[ch, i, j - 1] - 2 * data[ch, i, j + 1] + data[ch, i + 1, j - 1] - data[ch, i + 1, j + 1];
                        }
                    }
                }

                return result;
            }

            /// <summary>
            /// 計算GY的矩陣
            /// </summary>
            /// <returns>The gy.</returns>
            /// <param name="data">Data.</param>
            public int[,,] CalGY(int[,,] data){
                int[,,] result = new int[data.GetLength(0), data.GetLength(1) - 2, data.GetLength(2) - 2];

                for (int i = 1; i < data.GetLength(1) - 1; i++)
                {
                    for (int j = 1; j < data.GetLength(2) - 1; j++)
                    {
                        for (int ch = 0; ch < data.GetLength(0); ch++)
                        {

                            result[ch, i - 1, j - 1] = data[ch, i - 1, j - 1] + 2 * data[ch, i - 1, j] + data[ch, i - 1, j + 1] - data[ch, i + 1, j - 1] - 2 * data[ch, i + 1, j] - data[ch, i + 1, j + 1];
                        }
                    }
                }

                return result;
            }
        } 

        class BinaryImageOverlap{
            public BinaryImageOverlap(){}

            public int[,,] CalOverlap(int[,,] data, int door){
                
                SobelEdgeDetection SobelEdgeDetection = new SobelEdgeDetection();

                int[,,] GX = SobelEdgeDetection.CalGX(data);
                int[,,] GY = SobelEdgeDetection.CalGY(data);
                int[,,] result = SobelEdgeDetection.Sobel(data, door, GX, GY);

                for (int i = 0;i < result.GetLength(1);i++){
                    for (int j = 0; j < result.GetLength(2);j++){
                        //是白色
                        if(result[0,i,j] == 255){
                            result[0, i, j] = 0;
                            result[1, i, j] = 255;
                            result[2, i, j] = 0;
                        }
                    }
                }

                return result;
            }
        }
        
        class ConnectedComponent:Form{
            public ConnectedComponent(){}

            //計算物件並計算數量
            public int CountObject(int[,,] data){

                ParseToZero(data);
                int count = 0;
                List<int> tmpLi = new List<int>();
                for (int i = 0; i < data.GetLength(1); i++)
                {
                    for (int j = 0; j < data.GetLength(2); j++)
                    {
                        //是白色的在判斷
                        if (data[0, i, j] == 255)
                            continue;
                        else
                        {
                            bool mark = false;
                            int[] color = { 0, 0, 0 };
                            //List<int> tmpLi = new List<int>();
                            //掃描左上 上 左 左下是不是被另外標記了
                            for (int si = -1; si < 2; si++)
                            {
                                if ((si + i) < 0 || (si+i) >= data.GetLength(1))
                                    continue;
                                for (int sj = -1; sj < 2; sj++)
                                {
                                    if ((sj + j) < 0 || (sj + j) >= data.GetLength(2))
                                        continue;
                                    if (data[0, i + si, j + sj] != 255 & data[0, i + si, j + sj] != 0)
                                    {
                                        mark = true;
                                        color[0] = data[0, i + si, j + sj];
                                        color[1] = data[1, i + si, j + sj];
                                        color[2] = data[2, i +  si, j + sj];
                                    }
                                }
                            }

                            if (mark)
                            {
                                for (int si = -1; si < 2; si++)
                                {
                                    if ((si + i) < 0 || (si + i) >= data.GetLength(1))
                                        continue;
                                    for (int sj = -1; sj < 2; sj++)
                                    {
                                        if ((sj + j) < 0 || (sj + j) >= data.GetLength(2))
                                            continue;
                                        if (data[0, i + si, j + sj] != 255)
                                        {
                                            data[0, i + si, j + sj] = color[0];
                                            data[1, i + si, j + sj] = color[1];
                                            data[2, i + si, j + sj] = color[2];
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Random ran = new Random();//亂數種子

                                for (int k = 0; k < 3; k++)
                                {
                                    int ranNum = ran.Next(0, 255);
                                    while (tmpLi.Contains(ranNum))
                                    {
                                        ranNum = ran.Next(0, 255);
                                    }
                                    tmpLi.Add(ranNum);

                                    data[k, i, j] = ran.Next(0, 255);
                                }

                                count += 1;
                            }
                        }
                    }
                }
                return count;
            }

            //parse to 255 & 0
            private void ParseToZero(int[,,] data)
            {
                
                for(int j = 0; j < data.GetLength(1); j++)
                {
                    for(int k = 0; k < data.GetLength(2); k++)
                    {
                        if (data[0, j, k] < 128)
                        {
                            data[0, j, k] = 0;
                            data[1, j, k] = 0;
                            data[2, j, k] = 0;
                        }
                        else
                        {
                            data[0, j, k] = 255;
                            data[1, j, k] = 255;
                            data[2, j, k] = 255;
                        }
                    }
                 }
                

            }
        }

    }
}
