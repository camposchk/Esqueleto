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

    return imagem;
}

Bitmap Esqueleto(Bitmap imagem)
{
    //Coordenadas extremidades
    int xEsquerda = int.MaxValue;
    int xDireita = 0;
    int xAcima = 0;
    int xAbaixo = 0;

    int yEsquerda = 0;
    int yDireita = 0;
    int yAcima = int.MaxValue;
    int yAbaixo = 0;

    int count = 0;

    for (int j = 0; j < imagem.Height; j++)
    {
        int inicio = -1;
        int fim = 0;
        int soma = 0;

        for (int i = 0; i < imagem.Width; i++)
        {
            Color pixel = imagem.GetPixel(i, j);

            if (pixel == Color.FromArgb(255, 255, 0, 0))
            {
                imagem.SetPixel(i, j, Color.White);

                //Primeiro ponto vermelho encontrado
                if (inicio == -1)
                    inicio = i;
    
                fim = i;
                soma += i;
                count++;

                //Ponto mais a esquerda
                if (i < xEsquerda)
                {
                    xEsquerda = i;
                    yEsquerda = j;
                }

                //Ponto mais a direita
                if (i > xDireita)
                {
                    xDireita = i;
                    yDireita = j;
                }

                //Ponto mais acima
                if (j < yAcima)
                {
                    xAcima = i;
                    yAcima = j;
                }

                //Ponto mais abaixo
                if (j > yAbaixo)
                {
                    xAbaixo = i;
                    yAbaixo = j;
                }
            }
            else if (inicio != -1)
            {
                if (count > 40)
                {
                    int meio = (int)Math.Round((double)soma / count);
                    imagem.SetPixel(meio, j, Color.Black);
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

    for (int j = yAcima; j < yAbaixo; j++)
    {
        imagem.SetPixel(xAcima, j, Color.Red);
        count++;
    }

    int yTorso = count / 3;

    // Create a Graphics object from the Bitmap
    using (Graphics graphics = Graphics.FromImage(imagem))
    {
        // Set the pen properties for drawing the line
        using (Pen pen = new Pen(Color.Red, 2))
        {
            // Define the points to be connected
            Point point1 = new Point(xEsquerda, yEsquerda); // Replace x1 and y1 with your first point's coordinates
            Point point2 = new Point(xAcima, yTorso); // Replace x2 and y2 with your second point's coordinates
            Point point3 = new Point(xDireita, yDireita); // Replace x2 and y2 with your second point's coordinates

            // Draw a line connecting the two points
            graphics.DrawLine(pen, point1, point2);
            graphics.DrawLine(pen, point2, point3);
        }
    }

    return imagem;
}
