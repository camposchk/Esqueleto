using System;
using static System.Console;
using System.Drawing;
using System.Drawing.Imaging;

Bitmap imagemOriginal = new Bitmap("diferenca4.png");

Bitmap novaImagem = Cor(imagemOriginal);
novaImagem.Save("1.png");

Bitmap imagemFinal = Esqueleto(novaImagem);
imagemFinal.Save("1.png");

Bitmap Cor(Bitmap imagem)
{
    for(int i = 0; i < imagem.Height; i++) 
    {
        for(int j = 0; j < imagem.Width; j++)
        {
            Color pixel = imagem.GetPixel(j, i);
            // WriteLine($"Linha: {i}, pixel: {pixel}");
            if (pixel != Color.FromArgb(255, 255, 255, 255))
            {
                imagem.SetPixel(j, i, Color.Red);
                // fim = j;
                // count++;
            }
        }
        
        // WriteLine($"Linha: {i}, Count: {count}");
        // imagem.SetPixel((fim-count/2), i, Color.Red);
    }

    //Teste vertical (apagando esse for o código volta ao normal)
    for (int i = 0; i < imagem.Width; i++)
    {
        int inicio = -1;
        int fim = 0;
        int count = 0;

        for (int j = 0; j < imagem.Height; j++)
        {
            Color pixel = imagem.GetPixel(i, j);

            if (pixel == Color.FromArgb(255, 255, 0, 0))
            {
                // imagem.SetPixel(i, j, Color.White);
                if (inicio == -1)
                {
                    inicio = j;
                }
                count++;
            }
            if (inicio != -1)
            {
                if (pixel != Color.FromArgb(255, 255, 0, 0))
                {
                    if (count > 250 && count < 400)
                    {
                        fim = j;
                        int meio = (inicio + fim) / 2;
                        imagem.SetPixel(i, inicio, Color.Black);
                        imagem.SetPixel(i, fim, Color.Black);
                        imagem.SetPixel(i, meio-50, Color.Black);
                        inicio = -1;
                        count = 0;
                    }
                    else 
                        inicio = -1;
                }
            }
        }
    }

    return imagem;
}

Bitmap Esqueleto(Bitmap imagem)
{
    int pontoMaisAEsquerda = int.MaxValue;
    int pontoMaisADireita = 0;
    int pontoMaisAcima = int.MaxValue;
    int pontoMaisAbaixo = 0;

    int yPontoMaisAEsquerda = 0;
    int yPontoMaisADireita = 0;
    int xPontoMaisAcima = 0;
    int xPontoMaisAbaixo = 0;

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

                if (j < pontoMaisAEsquerda)
                {
                    pontoMaisAEsquerda = j;
                    yPontoMaisAEsquerda = i;
                }
                if (j > pontoMaisADireita)
                {
                    pontoMaisADireita = j;
                    yPontoMaisADireita = i;
                }
                if (i < pontoMaisAcima)
                {
                    pontoMaisAcima = i;
                    xPontoMaisAcima = j;
                }
                if (i > pontoMaisAbaixo)
                {
                    pontoMaisAbaixo = i;
                    xPontoMaisAbaixo = j;
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
        imagem.SetPixel(pontoMaisAEsquerda, yPontoMaisAEsquerda, Color.Red);
        imagem.SetPixel(pontoMaisADireita, yPontoMaisADireita, Color.Red);
        imagem.SetPixel(xPontoMaisAcima, pontoMaisAcima, Color.Red);
        imagem.SetPixel(xPontoMaisAbaixo, pontoMaisAbaixo, Color.Red);

    for(int i = )

    for (int i = pontoMaisAcima; i < pontoMaisAbaixo; i++)
    {
        imagem.SetPixel(xPontoMaisAcima, i, Color.Red);
    }

    for (int j = pontoMaisAEsquerda; j < pontoMaisADireita; j++)
    
    {
        imagem.SetPixel(j, yPontoMaisADireita, Color.Red);
    }

    return imagem;
}






// Bitmap detectarMao(Bitmap imagem)
// {

// }
