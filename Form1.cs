﻿using System;
using System.IO;
using System.Windows.Forms;

namespace Youtube
{
    public partial class blocoDeNotas : Form
    {
        private bool estaSalvo;
        private string textoSalvo;
        private string nomeArquivo;
        private string path;

        public blocoDeNotas()
        {
            InitializeComponent();
        }

        private void BlocoDeNotas_Load(object sender, EventArgs e)
        {
            estaSalvo = true;
            textoSalvo = txtTexto.Text;
            nomeArquivo = "Bloco de notas";
            path = "";
        }

        /*
         * Verifica se o texto possui alterações e altera o título
         * adicionando um '*', caso o esteja diferente do texto salvo
         * */
        private void TxtTexto_TextChanged(object sender, EventArgs e)
        {
            var textoAtual = txtTexto.Text;
            estaSalvo = false;

            if (textoAtual != textoSalvo && estaSalvo == false)
            {
                this.Text = "*" + nomeArquivo;
            }
            if (textoAtual == textoSalvo)
            {
                estaSalvo = true;
                this.Text = nomeArquivo;
            }
        }

        // Abre uma janela para que o user esoclha qual arquivo abrir
        private void MenuAbrirArquivo_Click(object sender, EventArgs e)
        {
            var abrirArquivo = new OpenFileDialog();
            abrirArquivo.Title = "Selecione um arquivo";
            abrirArquivo.Filter = "txt file|*txt";
            abrirArquivo.RestoreDirectory = true;

            // Pega o resultado de qual botão ele clicou
            var resultado = abrirArquivo.ShowDialog();

            // User escolheu um arquivo
            if (resultado == DialogResult.OK)
            {
                // Cria um leitor para transferir os dados para o programa atual
                using(var leitor = new StreamReader(abrirArquivo.FileName))
                {
                    txtTexto.Text = leitor.ReadToEnd();
                }
                nomeArquivo = abrirArquivo.SafeFileName;
                path = abrirArquivo.FileName;
                SalvarModificacoes();
            }
        }

        // Método que salva o texto e o título do bloco de notas
        private void SalvarModificacoes()
        {
            this.estaSalvo = true;
            this.Text = nomeArquivo;
            this.textoSalvo = txtTexto.Text;
        }

        private void MenuSalvar_Click(object sender, EventArgs e)
        {
            if (path != "")
            {
                SalvarArquivo();
            }
        }

        private void SalvarArquivo()
        {
            // Cria um escritor para sobrescrever os arquivos
            using(var escritor = new StreamWriter(this.path))
            {
                escritor.Write(txtTexto.Text);
            }
        }

        private void BlocoDeNotas_FormClosing(object sender, FormClosingEventArgs e)
        {
            /*
             * Caso o user feche a janela sem salvar as alterações, o programa
             * questiona se ele deseja salvar
             * */
            if (estaSalvo == false)
            {
                var botoes = MessageBoxButtons.YesNoCancel;
                var mensagem = "O programa não possui alterações salvas, deseja salvar?";
                var titulo = "Modificações pendentes";
                var icone = MessageBoxIcon.Warning;

                var resposta = MessageBox.Show(mensagem, titulo, botoes, icone);

                // Salva o arquivo
                if (resposta == DialogResult.Yes)
                {
                    SalvarArquivo();
                }
                else if (resposta == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
