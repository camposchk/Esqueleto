using System;
using static System.Console;
using System.Drawing;
using System.Drawing.Imaging;

//Imagem que estamos fazendo o esqueleto
Bitmap imagemOriginal = new("imagem.png");

DateTime dateTime = DateTime.Now;

//Imagem finalizada com o esqueleto
Bitmap imagemFinal = (Bitmap)Esqueleto(imagemOriginal);
var time = DateTime.Now - dateTime;

WriteLine(time.TotalMilliseconds);

imagemFinal.Save("Esqueleto.png");

static unsafe object Esqueleto(Bitmap imagem)
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

    var data = imagem.LockBits(
        new Rectangle(0, 0, imagem.Width, imagem.Height),
        ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb
    );

    byte* im = (byte*)data.Scan0.ToPointer();
    var stride = data.Stride;

    for (int j = 0; j < imagem.Height; j++)
    {
        int inicio = -1;
        int soma = 0;

        for (int i = 0; i < imagem.Width; i++)
        {
            int index = 3 * i + j * stride;
            byte b = im[index + 0]; 
            byte g = im[index + 1];
            byte r = im[index + 2];

            if (b == 0 && r == 0 && g == 0)
            {
                // imagem.SetPixel(i, j, Color.White);

                //Primeiro ponto preto encontrado
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

                int xMeio = (int)Math.Round((double)soma / count);
                int yMeio = j;

                inicio = -1;
                soma = 0;
                count = 0;
            }
        }
    }

    for (int j = yAcima; j < yAbaixo; j++)
    {
        count++;
    }

    int yTorso = count / 3;
    int yPerna = 3 * count / 5;

    int yAcimaMaoEsquerda = int.MaxValue;
    int yAbaixoMaoEsquerda = 0;

    for (int j = 0; j < imagem.Height; j++)
    {
        for (int i = xEsquerda; i < xEsquerda + 70; i++)
        {
            int index = 3 * i + j * stride;
            byte b = im[index + 0]; 
            byte g = im[index + 1];
            byte r = im[index + 2];

            if (b == 0 && r == 0 && g == 0)
            {
                if (j < yAcimaMaoEsquerda)
                    yAcimaMaoEsquerda = j;

                if (j > yAbaixoMaoEsquerda)
                    yAbaixoMaoEsquerda = j;
            }
        }
    }

    int yAcimaMaoDireita = int.MaxValue;
    int yAbaixoMaoDireita = 0;

    for (int j = 0; j < imagem.Height; j++)
    {
        for (int i = xDireita - 40 ; i < xDireita; i++)
        {
            int index = 3 * i + j * stride;
            byte b = im[index + 0]; 
            byte g = im[index + 1];
            byte r = im[index + 2];

            if (b == 0 && r == 0 && g == 0)
            {
                if (j < yAcimaMaoDireita)
                    yAcimaMaoDireita = j;

                if (j > yAbaixoMaoDireita)
                    yAbaixoMaoDireita = j;
            }
        }
    }

    int xEsquerdaCabeca = int.MaxValue;
    int xDireitaCabeca = 0;

    for (int j = yAcima; j < yAcima + 120; j++)
    {
        for (int i = xAcima - 70; i < xAcima + 70; i++)
        {
            int index = 3 * i + j * stride;
            byte b = im[index + 0]; 
            byte g = im[index + 1];
            byte r = im[index + 2];

            if (b == 0 && r == 0 && g == 0)
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

    int xMeioRetanguloCabeca = xDireitaCabeca - (xDireitaCabeca - xEsquerdaCabeca) / 2;

    int xCabeca = xMeioRetanguloCabeca;
    int yCabeca = yAcima;

    int xEsquerdaPerna = int.MaxValue;
    int xDireitaPerna = 0;

    for (int j = yPerna; j < yAbaixo; j++)
    {
        for (int i = 0; i < imagem.Width; i++)
        {
            int index = 3 * i + j * stride;
            byte b = im[index + 0]; 
            byte g = im[index + 1];
            byte r = im[index + 2];

            if (b == 0 && r == 0 && g == 0)
            {
                //Ponto mais a esquerda
                if (i < xEsquerdaPerna)
                    xEsquerdaPerna = i;

                //Ponto mais a direita
                if (i > xDireitaPerna)
                    xDireitaPerna = i;
            }
        }
    }
    int xMeioRetanguloPerna = xDireitaPerna - (xDireitaPerna - xEsquerdaPerna) / 2;

    int xMeioCintura = xMeioRetanguloPerna;
    int yMeioCintura = yPerna;

    int yAbaixoPernaEsquerda = 0;
    int yAbaixoPernaDireita = 0;

    int xAbaixoPernaEsquerda = 0;
    int xAbaixoPernaDireita = 0;

    for (int j = 0; j < imagem.Height; j++)
    {
        for (int i = xEsquerdaPerna; i < xMeioRetanguloPerna; i++)
        {
            int index = 3 * i + j * stride;
            byte b = im[index + 0]; 
            byte g = im[index + 1];
            byte r = im[index + 2];

            if (b == 0 && r == 0 && g == 0)
            {
                if (j > yAbaixoPernaEsquerda)
                    yAbaixoPernaEsquerda = j;
                    xAbaixoPernaEsquerda = i;
            }
        }
    }

    for (int j = 0; j < imagem.Height; j++)
    {
        for (int i = xMeioRetanguloPerna; i < xDireitaPerna; i++)
        {
            int index = 3 * i + j * stride;
            byte b = im[index + 0]; 
            byte g = im[index + 1];
            byte r = im[index + 2];

            if (b == 0 && r == 0 && g == 0)
            {
                if (j > yAbaixoPernaDireita)
                    yAbaixoPernaDireita = j;
                    xAbaixoPernaDireita = i;
            }
        }
    }

    int xPeEsquerdo = xAbaixoPernaEsquerda;
    int yPeEsquerdo = yAbaixoPernaEsquerda;

    int xPeDireito = xAbaixoPernaDireita;
    int yPeDireito = yAbaixoPernaDireita;

    int xJoelhoEsquerdo = xAbaixoPernaEsquerda;
    int yJoelhoEsquerdo = yAbaixoPernaEsquerda - (yAbaixoPernaEsquerda - yPerna) / 2;

    int xJoelhoDireito = xAbaixoPernaDireita;
    int yJoelhoDireito = yAbaixoPernaDireita - (yAbaixoPernaDireita - yPerna) / 2;

    float fatorInterpolacao = (float)(yTorso - yCabeca) / (yMeioCintura - yCabeca);
    int xInterpolado = xCabeca + (int)((xMeioCintura - xCabeca) * fatorInterpolacao);

    int xTorso = xInterpolado;
    // int yTorso = yTorso;

    int xMeioBracoEsquerdo = (xEsquerda + xTorso) / 2;
    int yMeioBracoEsquerdo = (yEsquerda + yTorso) / 2;

    int xMeioBracoDireito = (xDireita + xTorso) / 2;
    int yMeioBracoDireito = (yDireita + yTorso) / 2;

    int xEsqInfCabeca = xEsquerdaCabeca;
    int yEsqInfCabeca = yAcima + 120;

    int xDirInfCabeca = xDireitaCabeca;
    int yDirInfCabeca = yAcima + 120;

    int xOmbroEsquerdo = (xMeioBracoEsquerdo + xEsqInfCabeca + xTorso) / 3;
    int yOmbroEsquerdo = (yMeioBracoEsquerdo + yEsqInfCabeca + yTorso) / 3;

    int xOmbroDireito = (xMeioBracoDireito + xDirInfCabeca + xTorso) / 3;
    int yOmbroDireito = (yMeioBracoDireito + yDirInfCabeca + yTorso) / 3;

    int xMeioMaoOmbroEsquerdo = (xEsquerda + xOmbroEsquerdo) / 2;
    int yMeioMaoOmbroEsquerdo = yEsquerda;

    int yAbaixoCotoveloEsquerdo = 0;
    int yAcimaCotoveloEsquerdo = int.MaxValue;

    for(int j = 0; j < imagem.Height; j++)
    {   
        int index = 3 * xMeioMaoOmbroEsquerdo + j * stride;
        byte b = im[index + 0]; 
        byte g = im[index + 1];
        byte r = im[index + 2];

        if (b == 0 && r == 0 && g == 0)
        {
            if(j > yAbaixoCotoveloEsquerdo)
                yAbaixoCotoveloEsquerdo = j;      

            if(j < yAcimaCotoveloEsquerdo)
                yAcimaCotoveloEsquerdo = j;       
        }
        
    }

    int xCotoveloEsquerdo = xMeioMaoOmbroEsquerdo;
    int yCotoveloEsquerdo = yAbaixoCotoveloEsquerdo - (yAbaixoCotoveloEsquerdo - yAcimaCotoveloEsquerdo) / 2;

    int xMeioMaoOmbroDireito = (xDireita + xOmbroDireito) / 2;
    int yMeioMaoOmbroDireito = yDireita;

    int yAbaixoCotoveloDireito = 0;
    int yAcimaCotoveloDireito = int.MaxValue;

    for(int j = 0; j < imagem.Height; j++)
    {   
        int index = 3 * xMeioMaoOmbroDireito + j * stride;
        byte b = im[index + 0]; 
        byte g = im[index + 1];
        byte r = im[index + 2];
        if (b == 0 && r == 0 && g == 0)
        {
            if(j > yAbaixoCotoveloDireito)
                yAbaixoCotoveloDireito = j;      

            if(j < yAcimaCotoveloDireito)
                yAcimaCotoveloDireito = j;       
        }
        
    }

    int xCotoveloDireito = xMeioMaoOmbroDireito;
    int yCotoveloDireito = yAbaixoCotoveloDireito - (yAbaixoCotoveloDireito - yAcimaCotoveloDireito) / 2;

    imagem.UnlockBits(data);

    using Graphics graphics = Graphics.FromImage(imagem);

    using Pen pen = new(Color.Red, 2);

    Point pointCabeca = new(xCabeca, yCabeca);

    Point pointEsquerda = new(xEsquerda, yEsquerda);
    Point pointDireita = new(xDireita, yDireita);

    Point pointCotoveloEsquerdo = new(xCotoveloEsquerdo, yCotoveloEsquerdo);
    Point pointCotoveloDireito = new(xCotoveloDireito, yCotoveloDireito);

    Point pointOmbroEsquerdo = new(xOmbroEsquerdo, yOmbroEsquerdo);
    Point pointOmbroDireito = new(xOmbroDireito, yOmbroDireito);

    Point pointTorso = new(xTorso, yTorso);

    Point pointCintura = new(xMeioCintura, yMeioCintura);

    Point pointJoelhoEsquerdo = new(xJoelhoEsquerdo, yJoelhoEsquerdo);
    Point pointJoelhoDireito = new(xJoelhoDireito, yJoelhoDireito);

    Point pointPeEsquerdo = new(xPeEsquerdo, yPeEsquerdo);
    Point pointPeDireito = new(xPeDireito, yPeDireito);

    Point[] VerticalEsquerdo = 
    {
        pointCabeca,
        pointCintura,
        pointJoelhoEsquerdo,
        pointPeEsquerdo
    };

    Point[] VerticalDireito = 
    {
        pointCabeca,
        pointCintura,
        pointJoelhoDireito,
        pointPeDireito
    };

    Point[] Horizontal = 
    {
        pointEsquerda,
        pointCotoveloEsquerdo,
        pointOmbroEsquerdo,
        pointTorso,
        pointOmbroDireito,
        pointCotoveloDireito,
        pointDireita,
    };

    graphics.DrawLines(pen, Horizontal);
    graphics.DrawLines(pen, VerticalEsquerdo);
    graphics.DrawLines(pen, VerticalDireito);

    return imagem;
}
    

