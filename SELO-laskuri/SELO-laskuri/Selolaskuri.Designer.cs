﻿namespace Selolaskuri
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
            this.nykyinenSelo_input = new System.Windows.Forms.TextBox();
            this.Pelimäärä = new System.Windows.Forms.Label();
            this.pelimaara_input = new System.Windows.Forms.TextBox();
            this.VastustajanVahvuusluku_teksti = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Laske_button = new System.Windows.Forms.Button();
            this.UusiSELO_teksti = new System.Windows.Forms.Label();
            this.uusiSelo_output = new System.Windows.Forms.TextBox();
            this.Kayta_uutta_button = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.odotustulos_output = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.kerroin_output = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.uusi_pelimaara_output = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.seloEro_output = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.keskivahvuus_output = new System.Windows.Forms.TextBox();
            this.tulos_output = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.uusiSelo_diff_output = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.miettimisaika_vah90_Button = new System.Windows.Forms.RadioButton();
            this.miettimisaika_60_89_Button = new System.Windows.Forms.RadioButton();
            this.miettimisaika_11_59_Button = new System.Windows.Forms.RadioButton();
            this.miettimisaika_enint10_Button = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tulosVoitto_Button = new System.Windows.Forms.RadioButton();
            this.tulosTasapeli_Button = new System.Windows.Forms.RadioButton();
            this.tulosTappio_Button = new System.Windows.Forms.RadioButton();
            this.vaihteluvali_output = new System.Windows.Forms.TextBox();
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
            // nykyinenSelo_input
            // 
            this.nykyinenSelo_input.Location = new System.Drawing.Point(14, 118);
            this.nykyinenSelo_input.Name = "nykyinenSelo_input";
            this.nykyinenSelo_input.Size = new System.Drawing.Size(46, 20);
            this.nykyinenSelo_input.TabIndex = 1;
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
            // pelimaara_input
            // 
            this.pelimaara_input.Location = new System.Drawing.Point(148, 118);
            this.pelimaara_input.Name = "pelimaara_input";
            this.pelimaara_input.Size = new System.Drawing.Size(46, 20);
            this.pelimaara_input.TabIndex = 2;
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
            // uusiSelo_output
            // 
            this.uusiSelo_output.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uusiSelo_output.Location = new System.Drawing.Point(163, 363);
            this.uusiSelo_output.Name = "uusiSelo_output";
            this.uusiSelo_output.ReadOnly = true;
            this.uusiSelo_output.Size = new System.Drawing.Size(56, 29);
            this.uusiSelo_output.TabIndex = 12;
            this.uusiSelo_output.TabStop = false;
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
            this.label6.Text = "C#/.NET 5.4.2018 Ismo Suihko github/isuihko";
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
            // odotustulos_output
            // 
            this.odotustulos_output.Location = new System.Drawing.Point(308, 422);
            this.odotustulos_output.Name = "odotustulos_output";
            this.odotustulos_output.ReadOnly = true;
            this.odotustulos_output.Size = new System.Drawing.Size(46, 20);
            this.odotustulos_output.TabIndex = 17;
            this.odotustulos_output.TabStop = false;
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
            // kerroin_output
            // 
            this.kerroin_output.Location = new System.Drawing.Point(308, 441);
            this.kerroin_output.Name = "kerroin_output";
            this.kerroin_output.ReadOnly = true;
            this.kerroin_output.Size = new System.Drawing.Size(46, 20);
            this.kerroin_output.TabIndex = 19;
            this.kerroin_output.TabStop = false;
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
            // uusi_pelimaara_output
            // 
            this.uusi_pelimaara_output.Location = new System.Drawing.Point(163, 398);
            this.uusi_pelimaara_output.Name = "uusi_pelimaara_output";
            this.uusi_pelimaara_output.ReadOnly = true;
            this.uusi_pelimaara_output.Size = new System.Drawing.Size(56, 20);
            this.uusi_pelimaara_output.TabIndex = 21;
            this.uusi_pelimaara_output.TabStop = false;
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
            // seloEro_output
            // 
            this.seloEro_output.Location = new System.Drawing.Point(308, 403);
            this.seloEro_output.Name = "seloEro_output";
            this.seloEro_output.ReadOnly = true;
            this.seloEro_output.Size = new System.Drawing.Size(46, 20);
            this.seloEro_output.TabIndex = 23;
            this.seloEro_output.TabStop = false;
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
            // keskivahvuus_output
            // 
            this.keskivahvuus_output.Location = new System.Drawing.Point(163, 424);
            this.keskivahvuus_output.Name = "keskivahvuus_output";
            this.keskivahvuus_output.ReadOnly = true;
            this.keskivahvuus_output.Size = new System.Drawing.Size(56, 20);
            this.keskivahvuus_output.TabIndex = 26;
            this.keskivahvuus_output.TabStop = false;
            // 
            // tulos_output
            // 
            this.tulos_output.Location = new System.Drawing.Point(163, 441);
            this.tulos_output.Name = "tulos_output";
            this.tulos_output.ReadOnly = true;
            this.tulos_output.Size = new System.Drawing.Size(56, 20);
            this.tulos_output.TabIndex = 27;
            this.tulos_output.TabStop = false;
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
            // uusiSelo_diff_output
            // 
            this.uusiSelo_diff_output.Location = new System.Drawing.Point(225, 368);
            this.uusiSelo_diff_output.Name = "uusiSelo_diff_output";
            this.uusiSelo_diff_output.ReadOnly = true;
            this.uusiSelo_diff_output.Size = new System.Drawing.Size(37, 20);
            this.uusiSelo_diff_output.TabIndex = 29;
            this.uusiSelo_diff_output.TabStop = false;
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
            // miettimisaika_vah90_Button
            // 
            this.miettimisaika_vah90_Button.AutoSize = true;
            this.miettimisaika_vah90_Button.Checked = true;
            this.miettimisaika_vah90_Button.Location = new System.Drawing.Point(3, 14);
            this.miettimisaika_vah90_Button.Name = "miettimisaika_vah90_Button";
            this.miettimisaika_vah90_Button.Size = new System.Drawing.Size(80, 17);
            this.miettimisaika_vah90_Button.TabIndex = 9;
            this.miettimisaika_vah90_Button.TabStop = true;
            this.miettimisaika_vah90_Button.Text = "väh. 90 min";
            this.miettimisaika_vah90_Button.UseVisualStyleBackColor = true;
            this.miettimisaika_vah90_Button.CheckedChanged += new System.EventHandler(this.miettimisaika_vah90_Button_CheckedChanged);
            // 
            // miettimisaika_60_89_Button
            // 
            this.miettimisaika_60_89_Button.AutoSize = true;
            this.miettimisaika_60_89_Button.Location = new System.Drawing.Point(84, 14);
            this.miettimisaika_60_89_Button.Name = "miettimisaika_60_89_Button";
            this.miettimisaika_60_89_Button.Size = new System.Drawing.Size(71, 17);
            this.miettimisaika_60_89_Button.TabIndex = 32;
            this.miettimisaika_60_89_Button.TabStop = true;
            this.miettimisaika_60_89_Button.Text = "60-89 min";
            this.miettimisaika_60_89_Button.UseVisualStyleBackColor = true;
            this.miettimisaika_60_89_Button.CheckedChanged += new System.EventHandler(this.miettimisaika_60_89_Button_CheckedChanged);
            // 
            // miettimisaika_11_59_Button
            // 
            this.miettimisaika_11_59_Button.AutoSize = true;
            this.miettimisaika_11_59_Button.Location = new System.Drawing.Point(156, 14);
            this.miettimisaika_11_59_Button.Name = "miettimisaika_11_59_Button";
            this.miettimisaika_11_59_Button.Size = new System.Drawing.Size(71, 17);
            this.miettimisaika_11_59_Button.TabIndex = 33;
            this.miettimisaika_11_59_Button.TabStop = true;
            this.miettimisaika_11_59_Button.Text = "11-59 min";
            this.miettimisaika_11_59_Button.UseVisualStyleBackColor = true;
            this.miettimisaika_11_59_Button.CheckedChanged += new System.EventHandler(this.miettimisaika_11_59_Button_CheckedChanged);
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
            this.panel1.Controls.Add(this.miettimisaika_vah90_Button);
            this.panel1.Controls.Add(this.miettimisaika_enint10_Button);
            this.panel1.Controls.Add(this.miettimisaika_60_89_Button);
            this.panel1.Controls.Add(this.miettimisaika_11_59_Button);
            this.panel1.Location = new System.Drawing.Point(79, 49);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(317, 40);
            this.panel1.TabIndex = 35;
            // 
            // tulosVoitto_Button
            // 
            this.tulosVoitto_Button.AutoSize = true;
            this.tulosVoitto_Button.Location = new System.Drawing.Point(27, 284);
            this.tulosVoitto_Button.Name = "tulosVoitto_Button";
            this.tulosVoitto_Button.Size = new System.Drawing.Size(69, 17);
            this.tulosVoitto_Button.TabIndex = 6;
            this.tulosVoitto_Button.TabStop = true;
            this.tulosVoitto_Button.Text = "1 = voitto";
            this.tulosVoitto_Button.UseVisualStyleBackColor = true;
            this.tulosVoitto_Button.Enter += new System.EventHandler(this.tulosVoitto_Button_Enter);
            // 
            // tulosTasapeli_Button
            // 
            this.tulosTasapeli_Button.AutoSize = true;
            this.tulosTasapeli_Button.Location = new System.Drawing.Point(27, 265);
            this.tulosTasapeli_Button.Name = "tulosTasapeli_Button";
            this.tulosTasapeli_Button.Size = new System.Drawing.Size(90, 17);
            this.tulosTasapeli_Button.TabIndex = 5;
            this.tulosTasapeli_Button.TabStop = true;
            this.tulosTasapeli_Button.Text = "1/2 = tasapeli";
            this.tulosTasapeli_Button.UseVisualStyleBackColor = true;
            this.tulosTasapeli_Button.Enter += new System.EventHandler(this.tulosTasapeli_Button_Enter);
            // 
            // tulosTappio_Button
            // 
            this.tulosTappio_Button.AutoSize = true;
            this.tulosTappio_Button.Location = new System.Drawing.Point(27, 246);
            this.tulosTappio_Button.Name = "tulosTappio_Button";
            this.tulosTappio_Button.Size = new System.Drawing.Size(72, 17);
            this.tulosTappio_Button.TabIndex = 4;
            this.tulosTappio_Button.TabStop = true;
            this.tulosTappio_Button.Text = "0 = tappio";
            this.tulosTappio_Button.UseVisualStyleBackColor = true;
            this.tulosTappio_Button.Enter += new System.EventHandler(this.tulosTappio_Button_Enter);
            // 
            // vaihteluvali_output
            // 
            this.vaihteluvali_output.Location = new System.Drawing.Point(278, 368);
            this.vaihteluvali_output.Name = "vaihteluvali_output";
            this.vaihteluvali_output.ReadOnly = true;
            this.vaihteluvali_output.Size = new System.Drawing.Size(73, 20);
            this.vaihteluvali_output.TabIndex = 65;
            this.vaihteluvali_output.TabStop = false;
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
            this.Controls.Add(this.vaihteluvali_output);
            this.Controls.Add(this.tulosTappio_Button);
            this.Controls.Add(this.tulosTasapeli_Button);
            this.Controls.Add(this.tulosVoitto_Button);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.uusiSelo_diff_output);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.tulos_output);
            this.Controls.Add(this.keskivahvuus_output);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.seloEro_output);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.uusi_pelimaara_output);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.kerroin_output);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.odotustulos_output);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Kayta_uutta_button);
            this.Controls.Add(this.uusiSelo_output);
            this.Controls.Add(this.UusiSELO_teksti);
            this.Controls.Add(this.Laske_button);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.VastustajanVahvuusluku_teksti);
            this.Controls.Add(this.pelimaara_input);
            this.Controls.Add(this.Pelimäärä);
            this.Controls.Add(this.nykyinenSelo_input);
            this.Controls.Add(this.OmaVahvuusluku_teksti);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximumSize = new System.Drawing.Size(425, 550);
            this.MinimumSize = new System.Drawing.Size(425, 550);
            this.Name = "Form1";
            this.Text = "Selolaskuri v. 1.0.0.19";
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
        private System.Windows.Forms.TextBox nykyinenSelo_input;
        private System.Windows.Forms.Label Pelimäärä;
        private System.Windows.Forms.TextBox pelimaara_input;
        private System.Windows.Forms.Label VastustajanVahvuusluku_teksti;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button Laske_button;
        private System.Windows.Forms.Label UusiSELO_teksti;
        private System.Windows.Forms.TextBox uusiSelo_output;
        private System.Windows.Forms.Button Kayta_uutta_button;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox odotustulos_output;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox kerroin_output;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox uusi_pelimaara_output;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox seloEro_output;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox keskivahvuus_output;
        private System.Windows.Forms.TextBox tulos_output;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox uusiSelo_diff_output;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.RadioButton miettimisaika_vah90_Button;
        private System.Windows.Forms.RadioButton miettimisaika_60_89_Button;
        private System.Windows.Forms.RadioButton miettimisaika_11_59_Button;
        private System.Windows.Forms.RadioButton miettimisaika_enint10_Button;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton tulosVoitto_Button;
        private System.Windows.Forms.RadioButton tulosTasapeli_Button;
        private System.Windows.Forms.RadioButton tulosTappio_Button;
        private System.Windows.Forms.TextBox vaihteluvali_output;
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
