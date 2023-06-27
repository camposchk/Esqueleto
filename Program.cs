using System;
using static System.Console;
using System.Drawing;
using System.Drawing.Imaging;

//Imagem que estamos fazendo o esqueleto
Bitmap imagemOriginal = new Bitmap("imagem.png");

//Imagem após limpeza de pixels "sujos"
Bitmap novaImagem = Cor(imagemOriginal);
novaImagem.Save("Cor.png");

//Imagem finalizada com o esqueleto
Bitmap imagemFinal = Esqueleto(novaImagem);
imagemFinal.Save("Esqueleto.png");

//Função para a limpeza da imagem
Bitmap Cor(Bitmap imagem)
{
    //Percorrendo a imagem (horizontalmente)
    for (int j = 0; j < imagem.Height; j++)
    {
        for (int i = 0; i < imagem.Width; i++)
        {
            Color pixel = imagem.GetPixel(i, j);

            imagem.SetPixel(i, j, pixel == Color.FromArgb(255, 0, 0, 0) ? Color.Red : Color.White);
        }
    }

    //Percorrendo a imagem verticalmente
    // for (int i = 0; i < imagem.Width; i++)
    // {
    //     int inicio = -1;
    //     int fim = 0;
    //     int count = 0;

    //     for (int j = 0; j < imagem.Height; j++)
    //     {
    //         Color pixel = imagem.GetPixel(i, j);

    //         if (pixel == Color.FromArgb(255, 255, 0, 0))
    //         {
    //             // imagem.SetPixel(i, j, Color.White);
    //             if (inicio == -1)
    //                 inicio = j;
    //             count++;
    //         }
    //         if (inicio != -1)
    //         {
    //             if (pixel != Color.FromArgb(255, 255, 0, 0))
    //             {
    //                 if (count > 250 && count < 400)
    //                 {
    //                     fim = j;
    //                     int meio = (inicio + fim) / 2;
    //                     imagem.SetPixel(i, inicio, Color.Black);
    //                     imagem.SetPixel(i, fim, Color.Black);
    //                     imagem.SetPixel(i, meio - 50, Color.Black);
    //                     inicio = -1;
    //                     count = 0;
    //                 }
    //                 else
    //                     inicio = -1;
    //             }
    //         }
    //     }
    // }

    return imagem;
}

Bitmap Esqueleto(Bitmap imagem)
{
    int xEsquerda = int.MaxValue;
    int xDireita = 0;
    int xAcima = 0;
    int xAbaixo = 0;

    int yEsquerda = 0;
    int yDireita = 0;
    int yAcima = int.MaxValue;
    int yAbaixo = 0;

    for (int i = 0; i < imagem.Height; i++)
    {
        int inicio = -1;
        int fim = 0;
        int soma = 0;
        int count = 0;

        for (int j = 0; j < imagem.Width; j++)
        {
            Color pixel = imagem.GetPixel(j, i);

            if (pixel == Color.FromArgb(255, 255, 0, 0))
            {
                imagem.SetPixel(j, i, Color.White);
                if (inicio == -1)
                {
                    inicio = j;
                }
                fim = j;
                soma += j;
                count++;

                if (j < xEsquerda)
                {
                    xEsquerda = j;
                    yEsquerda = i;
                }

                if (j > xDireita)
                {
                    xDireita = j;
                    yDireita = i;
                }

                if (i < yAcima)
                {
                    yAcima = i;
                    xAcima = j;
                }
                
                if (i > yAbaixo)
                {
                    yAbaixo = i;
                    xAbaixo = j;
                }
            }
            else if (inicio != -1)
            {
                if (count > 40)
                {
                    int meio = (int)Math.Round((double)soma / count);
                    imagem.SetPixel(meio, i, Color.Black);
                }
                inicio = -1;
                soma = 0;
                count = 0;
            }
        }
    }

    // Adicionar pixels vermelhos nos pontos encontrados
    imagem.SetPixel(xEsquerda, yEsquerda, Color.Red);
    imagem.SetPixel(xDireita, yDireita, Color.Red);
    imagem.SetPixel(xAcima, yAcima, Color.Red);
    imagem.SetPixel(xAbaixo, yAbaixo, Color.Red);

    int countLinha = 0;

    for (int i = yAcima; i < yAbaixo; i++)
    {
        imagem.SetPixel(xAcima, i, Color.Red);
        countLinha++;
    }

    int torso = countLinha / 3;

    // Create a Graphics object from the Bitmap
    using (Graphics graphics = Graphics.FromImage(imagem))
    {
        // Set the pen properties for drawing the line
        using (Pen pen = new Pen(Color.Red, 2))
        {
            // Define the points to be connected
            Point point1 = new Point(xEsquerda, yEsquerda); // Replace x1 and y1 with your first point's coordinates
            Point point2 = new Point(xAcima, torso); // Replace x2 and y2 with your second point's coordinates
            Point point3 = new Point(xDireita, yDireita); // Replace x2 and y2 with your second point's coordinates

            // Draw a line connecting the two points
            graphics.DrawLine(pen, point1, point2);
            graphics.DrawLine(pen, point2, point3);
        }
    }

    return imagem;
}
