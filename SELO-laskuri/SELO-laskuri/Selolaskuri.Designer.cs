namespace Selolaskuri
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.OmaVahvuusluku_teksti = new System.Windows.Forms.Label();
            this.nykyinenSelo_in = new System.Windows.Forms.TextBox();
            this.Pelimäärä = new System.Windows.Forms.Label();
            this.pelimaara_in = new System.Windows.Forms.TextBox();
            this.VastustajanVahvuusluku_teksti = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Laske_button = new System.Windows.Forms.Button();
            this.UusiSELO_teksti = new System.Windows.Forms.Label();
            this.uusiSelo_out = new System.Windows.Forms.TextBox();
            this.Kayta_uutta_button = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.odotustulos_out = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.kerroin_out = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.uusiPelimaara_out = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.pisteEro_out = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.keskivahvuus_out = new System.Windows.Forms.TextBox();
            this.turnauksenTulos_out = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.selomuutos_out = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.miettimisaika_vah90_btn = new System.Windows.Forms.RadioButton();
            this.miettimisaika_60_89_btn = new System.Windows.Forms.RadioButton();
            this.miettimisaika_11_59_btn = new System.Windows.Forms.RadioButton();
            this.miettimisaika_enint10_Button = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tulosVoitto_btn = new System.Windows.Forms.RadioButton();
            this.tulosTasapeli_btn = new System.Windows.Forms.RadioButton();
            this.tulosTappio_btn = new System.Windows.Forms.RadioButton();
            this.vaihteluvali_out = new System.Windows.Forms.TextBox();
            this.TuloksetPistemaaranKanssa_teksti = new System.Windows.Forms.Label();
            this.vastustajanSelo_comboBox = new System.Windows.Forms.ComboBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ohjeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ohjeitaKäyttöönkeskenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.laskentakaavatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tietojaOhjelmastaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.suljeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // OmaVahvuusluku_teksti
            // 
            this.OmaVahvuusluku_teksti.AutoSize = true;
            this.OmaVahvuusluku_teksti.Location = new System.Drawing.Point(11, 102);
            this.OmaVahvuusluku_teksti.Name = "OmaVahvuusluku_teksti";
            this.OmaVahvuusluku_teksti.Size = new System.Drawing.Size(120, 13);
            this.OmaVahvuusluku_teksti.TabIndex = 52;
            this.OmaVahvuusluku_teksti.Text = "Oma SELO (1000-2999)";
            // 
            // nykyinenSelo_in
            // 
            this.nykyinenSelo_in.Location = new System.Drawing.Point(14, 118);
            this.nykyinenSelo_in.Name = "nykyinenSelo_in";
            this.nykyinenSelo_in.Size = new System.Drawing.Size(46, 20);
            this.nykyinenSelo_in.TabIndex = 1;
            // 
            // Pelimäärä
            // 
            this.Pelimäärä.AutoSize = true;
            this.Pelimäärä.Location = new System.Drawing.Point(145, 102);
            this.Pelimäärä.Name = "Pelimäärä";
            this.Pelimäärä.Size = new System.Drawing.Size(224, 13);
            this.Pelimäärä.TabIndex = 53;
            this.Pelimäärä.Text = "Oma pelimäärä (tyhjä tai jos uusi pelaaja: 0-10)";
            // 
            // pelimaara_in
            // 
            this.pelimaara_in.Location = new System.Drawing.Point(148, 118);
            this.pelimaara_in.Name = "pelimaara_in";
            this.pelimaara_in.Size = new System.Drawing.Size(46, 20);
            this.pelimaara_in.TabIndex = 2;
            // 
            // VastustajanVahvuusluku_teksti
            // 
            this.VastustajanVahvuusluku_teksti.AutoSize = true;
            this.VastustajanVahvuusluku_teksti.Location = new System.Drawing.Point(11, 148);
            this.VastustajanVahvuusluku_teksti.Name = "VastustajanVahvuusluku_teksti";
            this.VastustajanVahvuusluku_teksti.Size = new System.Drawing.Size(349, 13);
            this.VastustajanVahvuusluku_teksti.TabIndex = 54;
            this.VastustajanVahvuusluku_teksti.Text = "Vastustajan SELO. Tai monta tuloksineen: +1725 -1810 =1612 (tai 1612)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 230);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(359, 13);
            this.label3.TabIndex = 56;
            this.label3.Text = "Jos annettu yksi vahvuusluku numerona (esim. 1720), niin tuloksen valinta:";
            // 
            // Laske_button
            // 
            this.Laske_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Laske_button.Location = new System.Drawing.Point(69, 316);
            this.Laske_button.Name = "Laske_button";
            this.Laske_button.Size = new System.Drawing.Size(193, 25);
            this.Laske_button.TabIndex = 7;
            this.Laske_button.Text = "Laske uusi SELO";
            this.Laske_button.UseVisualStyleBackColor = true;
            this.Laske_button.Click += new System.EventHandler(this.Laske_button_Click);
            // 
            // UusiSELO_teksti
            // 
            this.UusiSELO_teksti.AutoSize = true;
            this.UusiSELO_teksti.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UusiSELO_teksti.Location = new System.Drawing.Point(10, 363);
            this.UusiSELO_teksti.Name = "UusiSELO_teksti";
            this.UusiSELO_teksti.Size = new System.Drawing.Size(102, 24);
            this.UusiSELO_teksti.TabIndex = 60;
            this.UusiSELO_teksti.Text = "Uusi SELO";
            // 
            // uusiSelo_out
            // 
            this.uusiSelo_out.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uusiSelo_out.Location = new System.Drawing.Point(163, 363);
            this.uusiSelo_out.Name = "uusiSelo_out";
            this.uusiSelo_out.ReadOnly = true;
            this.uusiSelo_out.Size = new System.Drawing.Size(56, 29);
            this.uusiSelo_out.TabIndex = 12;
            this.uusiSelo_out.TabStop = false;
            // 
            // Kayta_uutta_button
            // 
            this.Kayta_uutta_button.Location = new System.Drawing.Point(69, 467);
            this.Kayta_uutta_button.Name = "Kayta_uutta_button";
            this.Kayta_uutta_button.Size = new System.Drawing.Size(193, 25);
            this.Kayta_uutta_button.TabIndex = 8;
            this.Kayta_uutta_button.Text = "Käytä uutta SELOa jatkolaskennassa";
            this.Kayta_uutta_button.UseVisualStyleBackColor = true;
            this.Kayta_uutta_button.Click += new System.EventHandler(this.Kayta_uutta_button_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(45, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(304, 25);
            this.label5.TabIndex = 50;
            this.label5.Text = "Shakin vahvuusluvun laskenta";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(183, 497);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(217, 13);
            this.label6.TabIndex = 64;
            this.label6.Text = "C#/.NET 7.4.2018 Ismo Suihko github/isuihko";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(232, 424);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 13);
            this.label7.TabIndex = 58;
            this.label7.Text = "Odotustulos";
            // 
            // odotustulos_out
            // 
            this.odotustulos_out.Location = new System.Drawing.Point(308, 422);
            this.odotustulos_out.Name = "odotustulos_out";
            this.odotustulos_out.ReadOnly = true;
            this.odotustulos_out.Size = new System.Drawing.Size(46, 20);
            this.odotustulos_out.TabIndex = 17;
            this.odotustulos_out.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(232, 443);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 13);
            this.label8.TabIndex = 59;
            this.label8.Text = "Kerroin";
            // 
            // kerroin_out
            // 
            this.kerroin_out.Location = new System.Drawing.Point(308, 441);
            this.kerroin_out.Name = "kerroin_out";
            this.kerroin_out.ReadOnly = true;
            this.kerroin_out.Size = new System.Drawing.Size(46, 20);
            this.kerroin_out.TabIndex = 19;
            this.kerroin_out.TabStop = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(11, 398);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(76, 13);
            this.label9.TabIndex = 61;
            this.label9.Text = "Uusi pelimäärä";
            // 
            // uusiPelimaara_out
            // 
            this.uusiPelimaara_out.Location = new System.Drawing.Point(163, 398);
            this.uusiPelimaara_out.Name = "uusiPelimaara_out";
            this.uusiPelimaara_out.ReadOnly = true;
            this.uusiPelimaara_out.Size = new System.Drawing.Size(56, 20);
            this.uusiPelimaara_out.TabIndex = 21;
            this.uusiPelimaara_out.TabStop = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(232, 405);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(48, 13);
            this.label10.TabIndex = 57;
            this.label10.Text = "Piste-ero";
            // 
            // pisteEro_out
            // 
            this.pisteEro_out.Location = new System.Drawing.Point(308, 403);
            this.pisteEro_out.Name = "pisteEro_out";
            this.pisteEro_out.ReadOnly = true;
            this.pisteEro_out.Size = new System.Drawing.Size(46, 20);
            this.pisteEro_out.TabIndex = 23;
            this.pisteEro_out.TabStop = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(10, 424);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(133, 13);
            this.label11.TabIndex = 62;
            this.label11.Text = "Vastustajien keskivahvuus";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(10, 441);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(124, 13);
            this.label12.TabIndex = 63;
            this.label12.Text = "Ottelun/turnauksen tulos";
            // 
            // keskivahvuus_out
            // 
            this.keskivahvuus_out.Location = new System.Drawing.Point(163, 424);
            this.keskivahvuus_out.Name = "keskivahvuus_out";
            this.keskivahvuus_out.ReadOnly = true;
            this.keskivahvuus_out.Size = new System.Drawing.Size(56, 20);
            this.keskivahvuus_out.TabIndex = 26;
            this.keskivahvuus_out.TabStop = false;
            // 
            // turnauksenTulos_out
            // 
            this.turnauksenTulos_out.Location = new System.Drawing.Point(163, 441);
            this.turnauksenTulos_out.Name = "turnauksenTulos_out";
            this.turnauksenTulos_out.ReadOnly = true;
            this.turnauksenTulos_out.Size = new System.Drawing.Size(56, 20);
            this.turnauksenTulos_out.TabIndex = 27;
            this.turnauksenTulos_out.TabStop = false;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(11, 187);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(353, 13);
            this.label13.TabIndex = 55;
            this.label13.Text = "Montaa vahvuuslukua syötettäessä voitto +,   tasapeli = tai tyhjä,  tappio -";
            // 
            // selomuutos_out
            // 
            this.selomuutos_out.Location = new System.Drawing.Point(225, 368);
            this.selomuutos_out.Name = "selomuutos_out";
            this.selomuutos_out.ReadOnly = true;
            this.selomuutos_out.Size = new System.Drawing.Size(37, 20);
            this.selomuutos_out.TabIndex = 29;
            this.selomuutos_out.TabStop = false;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(11, 65);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(73, 13);
            this.label14.TabIndex = 51;
            this.label14.Text = "Miettimisaika: ";
            // 
            // miettimisaika_vah90_btn
            // 
            this.miettimisaika_vah90_btn.AutoSize = true;
            this.miettimisaika_vah90_btn.Checked = true;
            this.miettimisaika_vah90_btn.Location = new System.Drawing.Point(3, 14);
            this.miettimisaika_vah90_btn.Name = "miettimisaika_vah90_btn";
            this.miettimisaika_vah90_btn.Size = new System.Drawing.Size(80, 17);
            this.miettimisaika_vah90_btn.TabIndex = 9;
            this.miettimisaika_vah90_btn.TabStop = true;
            this.miettimisaika_vah90_btn.Text = "väh. 90 min";
            this.miettimisaika_vah90_btn.UseVisualStyleBackColor = true;
            this.miettimisaika_vah90_btn.CheckedChanged += new System.EventHandler(this.miettimisaika_vah90_Button_CheckedChanged);
            // 
            // miettimisaika_60_89_btn
            // 
            this.miettimisaika_60_89_btn.AutoSize = true;
            this.miettimisaika_60_89_btn.Location = new System.Drawing.Point(84, 14);
            this.miettimisaika_60_89_btn.Name = "miettimisaika_60_89_btn";
            this.miettimisaika_60_89_btn.Size = new System.Drawing.Size(71, 17);
            this.miettimisaika_60_89_btn.TabIndex = 32;
            this.miettimisaika_60_89_btn.TabStop = true;
            this.miettimisaika_60_89_btn.Text = "60-89 min";
            this.miettimisaika_60_89_btn.UseVisualStyleBackColor = true;
            this.miettimisaika_60_89_btn.CheckedChanged += new System.EventHandler(this.miettimisaika_60_89_Button_CheckedChanged);
            // 
            // miettimisaika_11_59_btn
            // 
            this.miettimisaika_11_59_btn.AutoSize = true;
            this.miettimisaika_11_59_btn.Location = new System.Drawing.Point(156, 14);
            this.miettimisaika_11_59_btn.Name = "miettimisaika_11_59_btn";
            this.miettimisaika_11_59_btn.Size = new System.Drawing.Size(71, 17);
            this.miettimisaika_11_59_btn.TabIndex = 33;
            this.miettimisaika_11_59_btn.TabStop = true;
            this.miettimisaika_11_59_btn.Text = "11-59 min";
            this.miettimisaika_11_59_btn.UseVisualStyleBackColor = true;
            this.miettimisaika_11_59_btn.CheckedChanged += new System.EventHandler(this.miettimisaika_11_59_Button_CheckedChanged);
            // 
            // miettimisaika_enint10_Button
            // 
            this.miettimisaika_enint10_Button.AutoSize = true;
            this.miettimisaika_enint10_Button.Location = new System.Drawing.Point(225, 14);
            this.miettimisaika_enint10_Button.Name = "miettimisaika_enint10_Button";
            this.miettimisaika_enint10_Button.Size = new System.Drawing.Size(85, 17);
            this.miettimisaika_enint10_Button.TabIndex = 34;
            this.miettimisaika_enint10_Button.TabStop = true;
            this.miettimisaika_enint10_Button.Text = "enint. 10 min";
            this.miettimisaika_enint10_Button.UseVisualStyleBackColor = true;
            this.miettimisaika_enint10_Button.CheckedChanged += new System.EventHandler(this.miettimisaika_enint10_Button_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.miettimisaika_vah90_btn);
            this.panel1.Controls.Add(this.miettimisaika_enint10_Button);
            this.panel1.Controls.Add(this.miettimisaika_60_89_btn);
            this.panel1.Controls.Add(this.miettimisaika_11_59_btn);
            this.panel1.Location = new System.Drawing.Point(79, 49);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(317, 40);
            this.panel1.TabIndex = 35;
            // 
            // tulosVoitto_btn
            // 
            this.tulosVoitto_btn.AutoSize = true;
            this.tulosVoitto_btn.Location = new System.Drawing.Point(27, 284);
            this.tulosVoitto_btn.Name = "tulosVoitto_btn";
            this.tulosVoitto_btn.Size = new System.Drawing.Size(69, 17);
            this.tulosVoitto_btn.TabIndex = 6;
            this.tulosVoitto_btn.TabStop = true;
            this.tulosVoitto_btn.Text = "1 = voitto";
            this.tulosVoitto_btn.UseVisualStyleBackColor = true;
            this.tulosVoitto_btn.Enter += new System.EventHandler(this.tulosVoitto_Button_Enter);
            // 
            // tulosTasapeli_btn
            // 
            this.tulosTasapeli_btn.AutoSize = true;
            this.tulosTasapeli_btn.Location = new System.Drawing.Point(27, 265);
            this.tulosTasapeli_btn.Name = "tulosTasapeli_btn";
            this.tulosTasapeli_btn.Size = new System.Drawing.Size(90, 17);
            this.tulosTasapeli_btn.TabIndex = 5;
            this.tulosTasapeli_btn.TabStop = true;
            this.tulosTasapeli_btn.Text = "1/2 = tasapeli";
            this.tulosTasapeli_btn.UseVisualStyleBackColor = true;
            this.tulosTasapeli_btn.Enter += new System.EventHandler(this.tulosTasapeli_Button_Enter);
            // 
            // tulosTappio_btn
            // 
            this.tulosTappio_btn.AutoSize = true;
            this.tulosTappio_btn.Location = new System.Drawing.Point(27, 246);
            this.tulosTappio_btn.Name = "tulosTappio_btn";
            this.tulosTappio_btn.Size = new System.Drawing.Size(72, 17);
            this.tulosTappio_btn.TabIndex = 4;
            this.tulosTappio_btn.TabStop = true;
            this.tulosTappio_btn.Text = "0 = tappio";
            this.tulosTappio_btn.UseVisualStyleBackColor = true;
            this.tulosTappio_btn.Enter += new System.EventHandler(this.tulosTappio_Button_Enter);
            // 
            // vaihteluvali_out
            // 
            this.vaihteluvali_out.Location = new System.Drawing.Point(278, 368);
            this.vaihteluvali_out.Name = "vaihteluvali_out";
            this.vaihteluvali_out.ReadOnly = true;
            this.vaihteluvali_out.Size = new System.Drawing.Size(73, 20);
            this.vaihteluvali_out.TabIndex = 65;
            this.vaihteluvali_out.TabStop = false;
            // 
            // TuloksetPistemaaranKanssa_teksti
            // 
            this.TuloksetPistemaaranKanssa_teksti.AutoSize = true;
            this.TuloksetPistemaaranKanssa_teksti.Location = new System.Drawing.Point(11, 204);
            this.TuloksetPistemaaranKanssa_teksti.Name = "TuloksetPistemaaranKanssa_teksti";
            this.TuloksetPistemaaranKanssa_teksti.Size = new System.Drawing.Size(282, 13);
            this.TuloksetPistemaaranKanssa_teksti.TabIndex = 66;
            this.TuloksetPistemaaranKanssa_teksti.Text = "Tai pistemäärä ja vastustajien SELOt: 1.5 1725 1810 1612";
            // 
            // vastustajanSelo_comboBox
            // 
            this.vastustajanSelo_comboBox.FormattingEnabled = true;
            this.vastustajanSelo_comboBox.Location = new System.Drawing.Point(14, 164);
            this.vastustajanSelo_comboBox.Name = "vastustajanSelo_comboBox";
            this.vastustajanSelo_comboBox.Size = new System.Drawing.Size(390, 21);
            this.vastustajanSelo_comboBox.TabIndex = 3;
            this.vastustajanSelo_comboBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.vastustajanSelo_combobox_KeyDown);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ohjeToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(409, 24);
            this.menuStrip1.TabIndex = 67;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ohjeToolStripMenuItem
            // 
            this.ohjeToolStripMenuItem.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ohjeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ohjeitaKäyttöönkeskenToolStripMenuItem,
            this.laskentakaavatToolStripMenuItem,
            this.tietojaOhjelmastaToolStripMenuItem,
            this.toolStripSeparator1,
            this.suljeToolStripMenuItem});
            this.ohjeToolStripMenuItem.Name = "ohjeToolStripMenuItem";
            this.ohjeToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.ohjeToolStripMenuItem.Text = "&Menu";
            // 
            // ohjeitaKäyttöönkeskenToolStripMenuItem
            // 
            this.ohjeitaKäyttöönkeskenToolStripMenuItem.Name = "ohjeitaKäyttöönkeskenToolStripMenuItem";
            this.ohjeitaKäyttöönkeskenToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.ohjeitaKäyttöönkeskenToolStripMenuItem.Text = "Ohjeita";
            this.ohjeitaKäyttöönkeskenToolStripMenuItem.Click += new System.EventHandler(this.ohjeitaToolStripMenuItem_Click);
            // 
            // laskentakaavatToolStripMenuItem
            // 
            this.laskentakaavatToolStripMenuItem.Name = "laskentakaavatToolStripMenuItem";
            this.laskentakaavatToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.laskentakaavatToolStripMenuItem.Text = "&Laskentakaavat";
            this.laskentakaavatToolStripMenuItem.Click += new System.EventHandler(this.laskentakaavatToolStripMenuItem_Click);
            // 
            // tietojaOhjelmastaToolStripMenuItem
            // 
            this.tietojaOhjelmastaToolStripMenuItem.Name = "tietojaOhjelmastaToolStripMenuItem";
            this.tietojaOhjelmastaToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.tietojaOhjelmastaToolStripMenuItem.Text = "&Tietoja ohjelmasta";
            this.tietojaOhjelmastaToolStripMenuItem.Click += new System.EventHandler(this.tietojaOhjelmastaToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(168, 6);
            // 
            // suljeToolStripMenuItem
            // 
            this.suljeToolStripMenuItem.Name = "suljeToolStripMenuItem";
            this.suljeToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.suljeToolStripMenuItem.Text = "&Sulje ohjelma";
            this.suljeToolStripMenuItem.Click += new System.EventHandler(this.suljeToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 512);
            this.Controls.Add(this.vastustajanSelo_comboBox);
            this.Controls.Add(this.TuloksetPistemaaranKanssa_teksti);
            this.Controls.Add(this.vaihteluvali_out);
            this.Controls.Add(this.tulosTappio_btn);
            this.Controls.Add(this.tulosTasapeli_btn);
            this.Controls.Add(this.tulosVoitto_btn);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.selomuutos_out);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.turnauksenTulos_out);
            this.Controls.Add(this.keskivahvuus_out);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.pisteEro_out);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.uusiPelimaara_out);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.kerroin_out);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.odotustulos_out);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Kayta_uutta_button);
            this.Controls.Add(this.uusiSelo_out);
            this.Controls.Add(this.UusiSELO_teksti);
            this.Controls.Add(this.Laske_button);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.VastustajanVahvuusluku_teksti);
            this.Controls.Add(this.pelimaara_in);
            this.Controls.Add(this.Pelimäärä);
            this.Controls.Add(this.nykyinenSelo_in);
            this.Controls.Add(this.OmaVahvuusluku_teksti);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximumSize = new System.Drawing.Size(425, 550);
            this.MinimumSize = new System.Drawing.Size(425, 550);
            this.Name = "Form1";
            this.Text = "Selolaskuri v. 1.0.1.0";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label OmaVahvuusluku_teksti;
        private System.Windows.Forms.TextBox nykyinenSelo_in;
        private System.Windows.Forms.Label Pelimäärä;
        private System.Windows.Forms.TextBox pelimaara_in;
        private System.Windows.Forms.Label VastustajanVahvuusluku_teksti;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button Laske_button;
        private System.Windows.Forms.Label UusiSELO_teksti;
        private System.Windows.Forms.TextBox uusiSelo_out;
        private System.Windows.Forms.Button Kayta_uutta_button;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox odotustulos_out;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox kerroin_out;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox uusiPelimaara_out;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox pisteEro_out;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox keskivahvuus_out;
        private System.Windows.Forms.TextBox turnauksenTulos_out;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox selomuutos_out;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.RadioButton miettimisaika_vah90_btn;
        private System.Windows.Forms.RadioButton miettimisaika_60_89_btn;
        private System.Windows.Forms.RadioButton miettimisaika_11_59_btn;
        private System.Windows.Forms.RadioButton miettimisaika_enint10_Button;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton tulosVoitto_btn;
        private System.Windows.Forms.RadioButton tulosTasapeli_btn;
        private System.Windows.Forms.RadioButton tulosTappio_btn;
        private System.Windows.Forms.TextBox vaihteluvali_out;
        private System.Windows.Forms.Label TuloksetPistemaaranKanssa_teksti;
        private System.Windows.Forms.ComboBox vastustajanSelo_comboBox;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ohjeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tietojaOhjelmastaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem laskentakaavatToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem suljeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ohjeitaKäyttöönkeskenToolStripMenuItem;
    }
}

