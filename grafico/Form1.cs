using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace grafico
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // ----------------------------
            // PARAMETRI CURVA DOMANDA
            // ----------------------------
            double B2 = 90;   // intercetta domanda
            double B3 = 4;    // coefficiente domanda

            // ----------------------------
            // PARAMETRI CURVA OFFERTA
            // ----------------------------
            double C1 = 10;     // intercetta offerta
            double C2 = 0.01;   // coefficiente offerta
            double C3 = 3;      // esponente offerta

            // ----------------------------
            // CONFIGURAZIONE GRAFICO
            // ----------------------------
            chart1.Series.Clear();
            chart1.ChartAreas[0].AxisX.Title = "Quantità";
            chart1.ChartAreas[0].AxisY.Title = "Prezzo";
            chart1.Titles.Clear();
            chart1.Titles.Add("Domanda e Offerta");

            Series domanda = new Series("Domanda");
            domanda.ChartType = SeriesChartType.Line;
            domanda.Color = Color.Green;
            domanda.BorderWidth = 3;

            Series offerta = new Series("Offerta");
            offerta.ChartType = SeriesChartType.Line;
            offerta.Color = Color.Blue;
            offerta.BorderWidth = 3;

            chart1.Series.Add(domanda);
            chart1.Series.Add(offerta);

            // ----------------------------
            // CONFIGURAZIONE TABELLA
            // ----------------------------
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();

            dataGridView1.Columns.Add("q", "Quantità");
            dataGridView1.Columns.Add("d", "Domanda");
            dataGridView1.Columns.Add("o", "Offerta");

            // ----------------------------
            // CALCOLO VALORI 0 → 20
            // ----------------------------
            for (int q = 0; q <= 20; q++)
            {
                double d = B2 - (B3 * q);                  // domanda
                double o = C1 + (C2 * Math.Pow(q, C3));    // offerta

                // Aggiunta punti al grafico
                domanda.Points.AddXY(q, d);
                offerta.Points.AddXY(q, o);


                // Aggiunta alla tabella
                dataGridView1.Rows.Add(q, d.ToString("0.00"), o.ToString("0.00"));
            }
        }
    }
}
