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
                // if (count > 40)
                // {
                int meio = (int)Math.Round((double)soma / count);
                imagem.SetPixel(meio, j, Color.Black);
                // }
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

    using (Graphics graphics = Graphics.FromImage(imagem))
    {
        // Set the pen properties for drawing the rectangles
        using (Pen pen = new(Color.Red, 2))
        {
            Point point1 = new(xEsquerda, yEsquerda);
            Point point2 = new(xAcima, yTorso);
            Point point3 = new(xDireita, yDireita);

            // Draw a line connecting the two points
            graphics.DrawLine(pen, point1, point2);
            graphics.DrawLine(pen, point2, point3);

            // Define the rectangles
            // Rectangle retangulo1 = new(xEsquerda, yEsquerda, 70, 50);
            Rectangle retangulo2 = new(xDireita - 40, yDireita - 15, 40, 45);

            // Draw the rectangles
            // graphics.DrawRectangle(pen, retangulo1);
            graphics.DrawRectangle(pen, retangulo2);

            // Calculate the bounding rectangle of the left hand
            int yAcimaMaoEsquerda = int.MaxValue;
            int yAbaixoMaoEsquerda = 0;

            for (int j = 0; j < imagem.Height; j++)
            {
                for (int i = xEsquerda; i < xEsquerda + 70; i++)
                {
                    Color pixel = imagem.GetPixel(i, j);

                    if (pixel == Color.FromArgb(255, 0, 0, 0))
                    {
                        // Topmost point
                        if (j < yAcimaMaoEsquerda)
                            yAcimaMaoEsquerda = j;

                        // Bottommost point
                        if (j > yAbaixoMaoEsquerda)
                            yAbaixoMaoEsquerda = j;
                    }
                }
            }

            // Define the bounding rectangle of the left hand
            Rectangle retanguloMaoEsquerda =
                new(xEsquerda, yAcimaMaoEsquerda, 70, yAbaixoMaoEsquerda - yAcimaMaoEsquerda);

            // Draw the bounding rectangle of the left hand
            graphics.DrawRectangle(pen, retanguloMaoEsquerda);

            // Calculate the bounding rectangle of the left hand
            int xEsquerdaCabeca = int.MaxValue;
            int xDireitaCabeca = 0;

            for (int j = yAcima; j < yAcima + 120; j++)
            {
                for (int i = xAcima - 70; i < xAcima + 70; i++)
                {
                    Color pixel = imagem.GetPixel(i, j);

                    if (pixel == Color.FromArgb(255, 0, 0, 0))
                    {
                        //Ponto mais a esquerda
                        if (i < xEsquerdaCabeca)
                            xEsquerdaCabeca = i;

                        //Ponto mais a direita
                        if (i > xDireitaCabeca)
                            xDireitaCabeca = i;
                    }
                }
            }

            Rectangle retanguloCabeca =
                new(xEsquerdaCabeca, yAcima, xDireitaCabeca - xEsquerdaCabeca, 120);

            graphics.DrawRectangle(pen, retanguloCabeca);
        }
    }

    return imagem;
}
