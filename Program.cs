﻿using System;
using static System.Console;
using System.Drawing;
using System.Drawing.Imaging;

//Imagem que estamos fazendo o esqueleto
Bitmap imagemOriginal = new Bitmap("imagem.png");

//Imagem finalizada com o esqueleto
DateTime dateTime = DateTime.Now;
Bitmap imagemFinal = Esqueleto(imagemOriginal);
var time = DateTime.Now - dateTime;

WriteLine(time.TotalMilliseconds);

imagemFinal.Save("Esqueleto.png");

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

            if (pixel == Color.FromArgb(255, 0, 0, 0))
            {
                // imagem.SetPixel(i, j, Color.White);

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
        // imagem.SetPixel(xAcima, j, Color.Red);
        count++;
    }

    int yTorso = count / 3;
    int yPerna = 3 * count / 5;

    using (Graphics graphics = Graphics.FromImage(imagem))
    {
        using (Pen pen = new(Color.Red, 2))
        {
            Point pointAcima = new(xAcima, yAcima);
            Point pointAbaixo = new(xAcima, yAbaixo);

            // graphics.DrawLine(pen, pointAcima, pointAbaixo);
            
            Point pointEsquerda = new(xEsquerda, yEsquerda);
            // Point pointTorso = new(xAcima, yTorso);
            Point pointDireita = new(xDireita, yDireita);

            // graphics.DrawLine(pen, pointEsquerda, pointTorso);
            // graphics.DrawLine(pen, pointTorso, pointDireita);

            // Rectangle retangulo1 = new(xEsquerda, yEsquerda, 70, 50);
            // Rectangle retangulo2 = new(xDireita - 40, yDireita - 15, 40, 45);

            // graphics.DrawRectangle(pen, retangulo1);
            // graphics.DrawRectangle(pen, retangulo2);

            int yAcimaMaoEsquerda = int.MaxValue;
            int yAbaixoMaoEsquerda = 0;

            for (int j = 0; j < imagem.Height; j++)
            {
                for (int i = xEsquerda; i < xEsquerda + 70; i++)
                {
                    Color pixel = imagem.GetPixel(i, j);

                    if (pixel == Color.FromArgb(255, 0, 0, 0))
                    {
                        if (j < yAcimaMaoEsquerda)
                            yAcimaMaoEsquerda = j;

                        if (j > yAbaixoMaoEsquerda)
                            yAbaixoMaoEsquerda = j;
                    }
                }
            }

            Rectangle retanguloMaoEsquerda = new(xEsquerda, yAcimaMaoEsquerda, 70, yAbaixoMaoEsquerda - yAcimaMaoEsquerda);

            graphics.DrawRectangle(pen, retanguloMaoEsquerda);

            int yAcimaMaoDireita = int.MaxValue;
            int yAbaixoMaoDireita = 0;

            for (int j = 0; j < imagem.Height; j++)
            {
                for (int i = xDireita - 40 ; i < xDireita; i++)
                {
                    Color pixel = imagem.GetPixel(i, j);

                    if (pixel == Color.FromArgb(255, 0, 0, 0))
                    {
                        if (j < yAcimaMaoDireita)
                            yAcimaMaoDireita = j;

                        if (j > yAbaixoMaoDireita)
                            yAbaixoMaoDireita = j;
                    }
                }
            }

            Rectangle retanguloMaoDireita = new(xDireita - 40, yAcimaMaoDireita, 40, yAbaixoMaoDireita - yAcimaMaoDireita);

            graphics.DrawRectangle(pen, retanguloMaoDireita);

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

            Rectangle retanguloCabeca = new(xEsquerdaCabeca, yAcima, xDireitaCabeca - xEsquerdaCabeca, 120);

            int xMeioRetanguloCabeca = xDireitaCabeca - (xDireitaCabeca - xEsquerdaCabeca) / 2;

            Point pointCabeca = new(xMeioRetanguloCabeca, yAcima);

            graphics.DrawRectangle(pen, retanguloCabeca);

            int xEsquerdaPerna = int.MaxValue;
            int xDireitaPerna = 0;

            for (int j = yPerna; j < yAbaixo; j++)
            {
                for (int i = 0; i < imagem.Width; i++)
                {
                    Color pixel = imagem.GetPixel(i, j);

                    if (pixel == Color.FromArgb(255, 0, 0, 0))
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

            Rectangle retanguloPerna = new(xEsquerdaPerna, yPerna, xDireitaPerna - xEsquerdaPerna, yAbaixo - yPerna);

            // graphics.DrawRectangle(pen, retanguloPerna);

            int xMeioRetanguloPerna = xDireitaPerna - (xDireitaPerna - xEsquerdaPerna) / 2;

            Point pointMeioCintura = new(xMeioRetanguloPerna, yPerna);

            int yAbaixoPernaEsquerda = 0;
            int yAbaixoPernaDireita = 0;

            int xAbaixoPernaEsquerda = 0;
            int xAbaixoPernaDireita = 0;

            for (int j = 0; j < imagem.Height; j++)
            {
                for (int i = xEsquerdaPerna; i < xMeioRetanguloPerna; i++)
                {
                    Color pixel = imagem.GetPixel(i, j);

                    if (pixel == Color.FromArgb(255, 0, 0, 0))
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
                    Color pixel = imagem.GetPixel(i, j);

                    if (pixel == Color.FromArgb(255, 0, 0, 0))
                    {
                        if (j > yAbaixoPernaDireita)
                            yAbaixoPernaDireita = j;
                            xAbaixoPernaDireita = i;
                    }
                }
            }

            Point pointPeEsquerdo = new(xAbaixoPernaEsquerda, yAbaixoPernaEsquerda);
            Point pointPeDireito = new(xAbaixoPernaDireita, yAbaixoPernaDireita);

            Point pointJoelhoEsquerdo = new(xAbaixoPernaEsquerda, yAbaixoPernaEsquerda - (yAbaixoPernaEsquerda - yPerna) / 2);
            Point pointJoelhoDireito = new(xAbaixoPernaDireita, yAbaixoPernaDireita - (yAbaixoPernaDireita - yPerna) / 2);

            float fatorInterpolacao = (float)(yTorso - pointCabeca.Y) / (pointMeioCintura.Y - pointCabeca.Y);
            int xInterpolado = pointCabeca.X + (int)((pointMeioCintura.X - pointCabeca.X) * fatorInterpolacao);
            Point pointTorso = new(xInterpolado, yTorso);

            // graphics.DrawLine(pen, pointMeioCintura, pointPeEsquerdo);
            // graphics.DrawLine(pen, pointMeioCintura, pointPeDireito);

            graphics.DrawLine(pen, pointPeEsquerdo, pointJoelhoEsquerdo);
            graphics.DrawLine(pen, pointPeDireito, pointJoelhoDireito);

            graphics.DrawLine(pen, pointJoelhoEsquerdo, pointMeioCintura);
            graphics.DrawLine(pen, pointJoelhoDireito, pointMeioCintura);

            graphics.DrawLine(pen, pointCabeca, pointMeioCintura);

            // graphics.DrawLine(pen, pointEsquerda, pointTorso);
            // graphics.DrawLine(pen, pointTorso, pointDireita);

            Point pointMeioBracoEsquerdo = new((pointEsquerda.X + pointTorso.X) / 2, (pointEsquerda.Y + pointTorso.Y) / 2);
            Point pointMeioBracoDireito = new((pointDireita.X + pointTorso.X) / 2, (pointDireita.Y + pointTorso.Y) / 2);

            Point pointEsqInfCabeca = new(xEsquerdaCabeca, yAcima + 120);
            Point pointDirInfCabeca = new(xDireitaCabeca, yAcima + 120);

            // graphics.DrawLine(pen, pointEsqInfCabeca, pointMeioBracoEsquerdo);
            // graphics.DrawLine(pen, pointMeioBracoDireito, pointDirInfCabeca);

            // graphics.DrawLine(pen, pointEsqInfCabeca, pointTorso);
            // graphics.DrawLine(pen, pointTorso, pointDirInfCabeca);

            int xOmbroEsquerdo = (int)((pointMeioBracoEsquerdo.X + pointEsqInfCabeca.X + pointTorso.X) / 3);
            int yOmbroEsquerdo = (int)((pointMeioBracoEsquerdo.Y + pointEsqInfCabeca.Y + pointTorso.Y) / 3);

            int xOmbroDireito = (int)((pointMeioBracoDireito.X + pointDirInfCabeca.X + pointTorso.X) / 3);
            int yOmbroDireito = (int)((pointMeioBracoDireito.Y + pointDirInfCabeca.Y + pointTorso.Y) / 3);

            Point pointOmbroEsquerdo = new(xOmbroEsquerdo, yOmbroEsquerdo);
            Point pointOmbroDireito = new(xOmbroDireito, yOmbroDireito);

            // graphics.DrawLine(pen, pointEsquerda, pointOmbroEsquerdo);
            // graphics.DrawLine(pen, pointDireita, pointOmbroDireito);

            Point pointMeioMaoOmbroEsquerdo = new((pointEsquerda.X + pointOmbroEsquerdo.X) / 2, yEsquerda);

            int yAbaixoCotoveloEsquerdo = 0;
            int yAcimaCotoveloEsquerdo = int.MaxValue;

            for(int j = 0; j < imagem.Height; j++)
            {   
                Color pixel = imagem.GetPixel(pointMeioMaoOmbroEsquerdo.X, j);
                if (pixel == Color.FromArgb(255, 0, 0, 0))
                {
                    if(j > yAbaixoCotoveloEsquerdo)
                        yAbaixoCotoveloEsquerdo = j;      

                    if(j < yAcimaCotoveloEsquerdo)
                        yAcimaCotoveloEsquerdo = j;       
                }
                
            }

            Point pointCotoveloEsquerdo = new(pointMeioMaoOmbroEsquerdo.X, yAbaixoCotoveloEsquerdo - (yAbaixoCotoveloEsquerdo - yAcimaCotoveloEsquerdo) / 2);
            graphics.DrawLine(pen, pointEsquerda, pointCotoveloEsquerdo);
            graphics.DrawLine(pen, pointCotoveloEsquerdo, pointOmbroEsquerdo);
            graphics.DrawLine(pen, pointOmbroEsquerdo, pointTorso);

            Point pointMeioMaoOmbroDireito = new((pointDireita.X + pointOmbroDireito.X) / 2, yDireita);

            int yAbaixoCotoveloDireito = 0;
            int yAcimaCotoveloDireito = int.MaxValue;

            for(int j = 0; j < imagem.Height; j++)
            {   
                Color pixel = imagem.GetPixel(pointMeioMaoOmbroDireito.X, j);
                if (pixel == Color.FromArgb(255, 0, 0, 0))
                {
                    if(j > yAbaixoCotoveloDireito)
                        yAbaixoCotoveloDireito = j;      

                    if(j < yAcimaCotoveloDireito)
                        yAcimaCotoveloDireito = j;       
                }
                
            }

            Point pointCotoveloDireito = new(pointMeioMaoOmbroDireito.X, yAbaixoCotoveloDireito - (yAbaixoCotoveloDireito - yAcimaCotoveloDireito) / 2);
            graphics.DrawLine(pen, pointDireita, pointCotoveloDireito);
            graphics.DrawLine(pen, pointCotoveloDireito, pointOmbroDireito);
            graphics.DrawLine(pen, pointOmbroDireito, pointTorso);
            

            Point pointPulso = new((pointEsquerda.X + pointCotoveloEsquerdo.X) / 2, yEsquerda);
            // graphics.DrawLine(pen, pointEsquerda, pointPulso);


            int yAbaixoPulsoEsquerdo = 0;

            for(int j = 0; j < imagem.Height; j++)
            {   
                Color pixel = imagem.GetPixel(pointPulso.X, j);
                // imagem.SetPixel(pointMeioMaoOmbroEsquerdo.X+1, j, Color.Blue);

                if (pixel == Color.FromArgb(255, 0, 0, 0))
                {
                    if(j > yAbaixoPulsoEsquerdo)
                        yAbaixoPulsoEsquerdo = j;       
                }
                
            }
            
            Point pointPulsoFim = new(pointPulso.X, yAbaixoPulsoEsquerdo);

            // graphics.DrawLine(pen, pointEsquerda, pointPulsoFim);


        }
    }

    return imagem;
}
