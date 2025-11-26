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

            // Registrazione evento TextChanged (opzionale)
            textBox1.TextChanged += TextBoxes_TextChanged;
            textBox2.TextChanged += TextBoxes_TextChanged;
            textBox3.TextChanged += TextBoxes_TextChanged;
            textBox4.TextChanged += TextBoxes_TextChanged;
            textBox5.TextChanged += TextBoxes_TextChanged;
            textBox6.TextChanged += TextBoxes_TextChanged;

            // All'avvio nascondi chart e table
            chart1.Visible = false;
            dataGridView1.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Nessun valore di default
        }

       

        private void TextBoxes_TextChanged(object sender, EventArgs e)
        {
            // Potresti voler non aggiornare alla digitazione
            // oppure tenerlo disattivato se preferisci solo con il bottone
            // AggiornaGraficoETabella();
        }

        private void AggiornaGraficoETabella()
        {
            if (!double.TryParse(textBox1.Text, out double B2)) return;
            if (!double.TryParse(textBox2.Text, out double B3)) return;
            if (!double.TryParse(textBox3.Text, out double C1)) return;
            if (!double.TryParse(textBox4.Text, out double C2)) return;
            if (!double.TryParse(textBox5.Text, out double C3)) return;
            if (!int.TryParse(textBox6.Text, out int Qmax) || Qmax < 0) return;

            // 2. Mostra grafico e tabella
            chart1.Visible = true;
            dataGridView1.Visible = true;

            // 3. Pulisci grafico e inizializza
            chart1.Series.Clear();
            chart1.Titles.Clear();
            chart1.Titles.Add("Domanda e Offerta");
            chart1.ChartAreas[0].AxisX.Title = "Quantità (q)";
            chart1.ChartAreas[0].AxisY.Title = "Prezzo";

            Series domanda = new Series("Domanda")
            {
                ChartType = SeriesChartType.Line,
                Color = Color.Green,
                BorderWidth = 2
            };
            Series offerta = new Series("Offerta")
            {
                ChartType = SeriesChartType.Line,
                Color = Color.Blue,
                BorderWidth = 2
            };
            chart1.Series.Add(domanda);
            chart1.Series.Add(offerta);

            // 4. Inizializza la tabella
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Add("q", "q");
            dataGridView1.Columns.Add("d", "Domanda");
            dataGridView1.Columns.Add("o", "Offerta");

            // 5. Popola grafico e tabella per q interi
            for (int q = 0; q <= Qmax; q++)
            {
                double d = B2 - (B3 * q);
                double o = C1 + (Math.Pow(q, C3) / C2);

                domanda.Points.AddXY(q, d);
                offerta.Points.AddXY(q, o);

                dataGridView1.Rows.Add(q, d.ToString("0.00"), o.ToString("0.00"));
            }

            // 6. Calcola approssimazione del punto di equilibrio (continua, passo fine)
            double bestQ = 0;
            double bestDiff = double.MaxValue;
            double priceAtEq = 0;
            double step = 0.1;  // passo per maggiore precisione

            for (double q = 0.0; q <= Qmax; q += step)
            {
                double d = B2 - (B3 * q);
                double o = C1 + (Math.Pow(q, C3) / C2);
                double diff = Math.Abs(d - o);
                if (diff < bestDiff)
                {
                    bestDiff = diff;
                    bestQ = q;
                    priceAtEq = (d + o) / 2.0;
                }
            }

            // 7. Se la differenza minima è ragionevole, segna punto di equilibrio
            // Alla fine del metodo AggiornaGraficoETabella(), dopo aver calcolato bestQ, priceAtEq, bestDiff:
            double soglia = 0.5;  // come prima
            if (bestDiff < soglia)
            {
                // Segna il punto sul grafico
                Series eq = new Series("Equilibrio")
                {
                    ChartType = SeriesChartType.Point,
                    MarkerStyle = MarkerStyle.Circle,
                    MarkerSize = 10,
                    Color = Color.Red
                };
                chart1.Series.Add(eq);
                eq.Points.AddXY(bestQ, priceAtEq);

                // Aggiorna la label con i valori di equilibrio
                label7.Text = $"Equilibrio ≃ q = {bestQ:F2}, p = {priceAtEq:F2}";
            }
            else
            {
                // Se non trova un buon equilibrio, svuota o informa
                label7.Text = "Nessun equilibrio trovato (auto ≠ offerta)";
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            AggiornaGraficoETabella();
        }
    }
}
