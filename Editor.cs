﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Lib;
using System.Collections;
using MHQuestHelperLib;

namespace mhfz_quest_editor
{
    public partial class Editor : Form
    {
        public Editor()
        {
            InitializeComponent();
            comboBox1.Items.AddRange(File.ReadAllLines("monster.txt"));
            comboBox2.Items.AddRange(File.ReadAllLines("item.txt"));
            comboBox_Lar1Boss_Monster.Items.AddRange(File.ReadAllLines("monster.txt"));
            comboBox17.Items.AddRange(File.ReadAllLines("monster.txt"));
            comboBox19.Items.AddRange(File.ReadAllLines("monster.txt"));
            comboBox21.Items.AddRange(File.ReadAllLines("monster.txt"));
            radioButton3.Checked = false;
            btn_save.Enabled = false;
        }

        byte[] TempData;

        private void btn_load_Click(object sender, EventArgs e)
        {
            try
            {
                Load_Quest();
                MessageBox.Show("加载成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载失败！：" + ex.ToString());
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                Save_Quest();
                MessageBox.Show("保存成功！");
            }
            catch(Exception ex)
            {
                MessageBox.Show("保存失败！："+ex.ToString());
            }
        }
        private void Load_Quest()
        {
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                string fileloc = openFileDialog1.FileName;
                byte[] ba = File.ReadAllBytes(fileloc);

                comboBox_Lar1Boss_SpawnArea.Items.Clear();
                comboBox18.Items.Clear();
                comboBox20.Items.Clear();
                comboBox22.Items.Clear();

                int MH2dosTopoffset = (BitConverter.ToInt32(ba, 0));
                //if (ba[0] == 192)
                if (true)
                {
                    textBox32.Text = BitConverter.ToString(ba).Replace("-", string.Empty);

                    TempData = new byte[ba.Length];
                    Array.Copy(ba, TempData, ba.Length);

                    int HaveAobj = BitConverter.ToInt16(ba, MH2dosTopoffset + 52);
                    int HaveBobj = BitConverter.ToInt16(ba, MH2dosTopoffset + 60);

                    List.Location.TryGetValue(ba[MH2dosTopoffset + 32], out string lcoation);        //location
                    textBox23.Text = lcoation.ToString();

                    List.Rank.TryGetValue(ba[92], out string rank);                 //carve rank
                    comboBox_LarBoss_Carve.Text = rank.ToString();

                    numericUpDown_fee.Value = BitConverter.ToInt32(ba, MH2dosTopoffset + 8);           //fee
                    numericUpDown_Penalty.Value = BitConverter.ToInt32(ba, MH2dosTopoffset + 16);           //penalty

                    List.ObjectiveType1.TryGetValue(BitConverter.ToInt32(ba, MH2dosTopoffset + 44), out string MainType);        //Load main obj type
                    comboBox_MainType.Text = MainType;
                    if (BitConverter.ToInt32(ba, MH2dosTopoffset + 44) == 2)
                    {
                        List.Item.TryGetValue(BitConverter.ToInt16(ba, MH2dosTopoffset + 48), out string MainObjTgt);        //harvest
                        textBox_MainTarget.Text = MainObjTgt;
                    }
                    else
                    {
                        List.Monster1.TryGetValue(BitConverter.ToInt16(ba, MH2dosTopoffset + 48), out string MainObjTgt);
                        textBox_MainTarget.Text = MainObjTgt;
                    }
                    numericUpDown_MainAmount.Value = BitConverter.ToInt16(ba, MH2dosTopoffset + 50);           //amount
                    numericUpDown_MainRewardMoney.Value = BitConverter.ToInt32(ba, MH2dosTopoffset + 12);           //money

                    List.ObjectiveType1.TryGetValue(BitConverter.ToInt32(ba, MH2dosTopoffset + 52), out string AType);
                    comboBox_SubAType.Text = AType;
                    if (BitConverter.ToInt32(ba, MH2dosTopoffset + 52) == 2)
                    {
                        List.Item.TryGetValue(BitConverter.ToInt16(ba, MH2dosTopoffset + 56), out string AObjTgt);
                        textBox_SubATarget.Text = AObjTgt;
                    }
                    else
                    {
                        List.Monster1.TryGetValue(BitConverter.ToInt16(ba, MH2dosTopoffset + 56), out string AObjTgt);
                        textBox_SubATarget.Text = AObjTgt;
                    }
                    numericUpDown_SubA_Amount.Value = BitConverter.ToInt16(ba, MH2dosTopoffset + 58);
                    numericUpDown_SubARewardMoney.Value = BitConverter.ToInt32(ba, MH2dosTopoffset + 20);

                    List.ObjectiveType1.TryGetValue(BitConverter.ToInt32(ba, MH2dosTopoffset + 60), out string BType);
                    comboBox_SubBType.Text = BType.ToString();

                    if (BitConverter.ToInt32(ba, MH2dosTopoffset + 60) == 2)
                    {
                        List.Item.TryGetValue(BitConverter.ToInt16(ba, MH2dosTopoffset + 64), out string BObjTgt);
                        textBox_SubBTarget.Text = BObjTgt;
                    }
                    else
                    {
                        List.Monster1.TryGetValue(BitConverter.ToInt16(ba, MH2dosTopoffset + 64), out string BObjTgt);
                        textBox_SubBTarget.Text = BObjTgt;
                    }
                    numericUpDown_SubB_Amount.Value = BitConverter.ToInt16(ba, MH2dosTopoffset + 66);       //amount
                    numericUpDown_SubBRewardMoney.Value = BitConverter.ToInt32(ba, MH2dosTopoffset + 24);       //money

                    if (BitConverter.ToInt32(ba, MH2dosTopoffset + 44) == 32772)     //if slay
                    {
                        numericUpDown_MainAmount.Value = numericUpDown_MainAmount.Value * 100;
                    }
                    if (BitConverter.ToInt32(ba, MH2dosTopoffset + 52) == 32772)
                    {
                        numericUpDown_SubA_Amount.Value = numericUpDown_SubA_Amount.Value * 100;
                    }
                    if (BitConverter.ToInt32(ba, MH2dosTopoffset + 60) == 32772)
                    {
                        numericUpDown_SubB_Amount.Value = numericUpDown_SubB_Amount.Value * 100;
                    }

                    //Load text
                    //int questStringsStart = BitConverter.ToInt32(ba, 48);       //go and get 4C80

                    //int questStringsStart = ba[0xC];       //go and get 4C80

                    int questStringsStart = BitConverter.ToInt32(ba, MH2dosTopoffset + 36);
                    //int readPointer = BitConverter.ToInt32(ba, questStringsStart);  //go text field
                    int readPointer = questStringsStart;


                    int pTitleAndName = BitConverter.ToInt32(ba, readPointer);
                    int pMainoObj = BitConverter.ToInt32(ba, readPointer + 4);
                    int pAObj = BitConverter.ToInt32(ba, readPointer + 8);
                    int pBObj = BitConverter.ToInt32(ba, readPointer + 12);
                    int pClearC = BitConverter.ToInt32(ba, readPointer + 16);
                    int pFailC = BitConverter.ToInt32(ba, readPointer + 20);
                    int pHirer = BitConverter.ToInt32(ba, (readPointer + 24));
                    int pText = BitConverter.ToInt32(ba, (readPointer + 28));

                    //int pTitleAndName = BitConverter.ToInt32(ba, (readPointer - 32));
                    //int pMainoObj = BitConverter.ToInt32(ba, (readPointer - 28));
                    //int pAObj = BitConverter.ToInt32(ba, (readPointer - 24));
                    //int pBObj = BitConverter.ToInt32(ba, (readPointer - 20));
                    //int pClearC = BitConverter.ToInt32(ba, (readPointer - 16));
                    //int pFailC = BitConverter.ToInt32(ba, (readPointer - 12));
                    //int pHirer = BitConverter.ToInt32(ba, (readPointer - 8));
                    //int pText = BitConverter.ToInt32(ba, (readPointer - 4));

                    byte[] tTitleAndName = File.ReadAllBytes(fileloc).Skip(pTitleAndName).Take(pMainoObj - pTitleAndName).ToArray();
                    string str = Encoding.GetEncoding("Shift_JIS").GetString(tTitleAndName);           //encode as shift jis

                    txtBox_TitleAndName.Text = Encoding.GetEncoding("Shift_JIS").GetString(File.ReadAllBytes(fileloc).Skip(pTitleAndName).Take(pMainoObj - pTitleAndName).ToArray()).Replace("\n", "\r\n");

                    txtBox_MainObjective.Text = Encoding.GetEncoding("Shift_JIS").GetString(File.ReadAllBytes(fileloc).Skip(pMainoObj).Take(pAObj - pMainoObj).ToArray());      //Main
                    string tB = Encoding.GetEncoding("Shift_JIS").GetString(File.ReadAllBytes(fileloc).Skip(pBObj).Take(pClearC - pBObj).ToArray());                //B
                    txtBox_SubBObjective.Text = tB; //B
                    string tA = txtBox_SubAObjective.Text = Encoding.GetEncoding("Shift_JIS").GetString(File.ReadAllBytes(fileloc).Skip(pAObj).Take(pBObj - pAObj).ToArray());
                    if (string.IsNullOrEmpty(tA))
                    {
                        txtBox_SubAObjective.Text = tB;
                    }

                    txtBox_ClearCondition.Text = Encoding.GetEncoding("Shift_JIS").GetString(File.ReadAllBytes(fileloc).Skip(pClearC).Take(pFailC - pClearC).ToArray());        //Clear
                    txtBox_FailCondition.Text = Encoding.GetEncoding("Shift_JIS").GetString(File.ReadAllBytes(fileloc).Skip(pFailC).Take(pHirer - pFailC).ToArray()).Replace("\n", "\r\n");

                    txtBox_Employer.Text = Encoding.GetEncoding("Shift_JIS").GetString(File.ReadAllBytes(fileloc).Skip(pHirer).Take(pText - pHirer).ToArray());
                    txtBox_Description.Text = Encoding.GetEncoding("Shift_JIS").GetString(File.ReadAllBytes(fileloc).Skip(pText).Take(ba.Length - pText).ToArray()).Replace("\n", "\r\n");

                    //Load supply item
                    int SupplyInfoStart = BitConverter.ToInt32(ba, 8);
                    byte[] SupplyInfoArray = File.ReadAllBytes(fileloc).Skip(SupplyInfoStart).Take(160).ToArray();

                    for (int i = 0; i < 40; i++)
                    {

                        if (i < 10)
                        {
                            for (int t = 0; t < 10; t += 1)
                            {
                                int u = 4 * t;
                                string SupplyItemID = SupplyInfoArray[u + 1].ToString("X2") + SupplyInfoArray[u].ToString("X2");
                                List.Item.TryGetValue(Convert.ToInt32(SupplyItemID, 16), out string SupplyItemName);
                                ((TextBox)this.Controls.Find("textBox10" + t.ToString(), true)[0]).Text = SupplyItemName;

                                int p = 4 * t + 2;
                                string SupplyItemAmount = SupplyInfoArray[p + 1].ToString("X2") + SupplyInfoArray[p].ToString("X2");
                                int SupplyItemAmount1 = (Convert.ToInt32(SupplyItemAmount, 16));
                                ((NumericUpDown)this.Controls.Find("numericUpDown10" + t.ToString(), true)[0]).Value = SupplyItemAmount1;
                            }

                        }
                        else if (i < 40)
                        {
                            for (int t = 10; t < 40; t += 1)
                            {
                                int u = 4 * t;
                                string SupplyItemID = SupplyInfoArray[u + 1].ToString("X2") + SupplyInfoArray[u].ToString("X2");
                                List.Item.TryGetValue(Convert.ToInt32(SupplyItemID, 16), out string SupplyItemName);
                                ((TextBox)this.Controls.Find("textBox1" + t.ToString(), true)[0]).Text = SupplyItemName;

                                int p = 4 * t + 2;
                                string SupplyItemAmount = SupplyInfoArray[p + 1].ToString("X2") + SupplyInfoArray[p].ToString("X2");
                                int SupplyItemAmount1 = (Convert.ToInt32(SupplyItemAmount, 16));
                                ((NumericUpDown)this.Controls.Find("numericUpDown1" + t.ToString(), true)[0]).Value = SupplyItemAmount1;
                            }
                        }
                    }

                    //Load reward item
                    int RewardInfoStart = BitConverter.ToInt32(ba, 12);
                    int MRewardPointer = 0;
                    int ARewardPointer = 0;
                    int BRewardPointer = 0;
                    int An1RewardPointer = 0;
                    int An2RewardPointer = 0;
                    byte[] RewardHeaderArray = ba.Skip(RewardInfoStart).Take(48).ToArray();
                    byte[] MRewardData = { };
                    byte[] ARewardData = { };
                    byte[] BRewardData = { };
                    byte[] An1RewardData = { };
                    byte[] An2RewardData = { };


                    for (int i = 0; i < 5; i++)
                    {
                        int RewardHeader = RewardHeaderArray[i * 8];
                        if (RewardHeader == 255)
                        {
                            break;
                        }
                        switch (RewardHeader)
                        {
                            case 0:
                                MRewardPointer = BitConverter.ToInt32(RewardHeaderArray, 4);
                                MRewardData = ba.Skip(MRewardPointer).Take(244).ToArray();
                                break;
                            case 1:
                                MRewardPointer = BitConverter.ToInt32(RewardHeaderArray, 4);
                                MRewardData = ba.Skip(MRewardPointer).Take(244).ToArray();
                                break;
                            case 2:
                                ARewardPointer = BitConverter.ToInt32(RewardHeaderArray, 12);
                                ARewardData = ba.Skip(ARewardPointer).Take(244).ToArray();
                                break;
                            case 3:
                                BRewardPointer = BitConverter.ToInt32(RewardHeaderArray, 20);
                                BRewardData = ba.Skip(BRewardPointer).Take(244).ToArray();
                                break;
                            case 4:
                                An1RewardPointer = BitConverter.ToInt32(RewardHeaderArray, 28);
                                An1RewardData = ba.Skip(An1RewardPointer).Take(244).ToArray();
                                break;
                            case 5:
                                An2RewardPointer = BitConverter.ToInt32(RewardHeaderArray, 36);
                                An2RewardData = ba.Skip(An2RewardPointer).Take(244).ToArray();
                                break;
                        }
                    }

                    for (int i = 10; i < 50; i++)
                    {
                        int MainRewardBase = (i - 10) * 6;

                        int MainRewardChance = MRewardData[MainRewardBase];
                        if (MainRewardChance == 255)
                        {
                            break;
                        }
                        ((NumericUpDown)this.Controls.Find("R12" + i.ToString(), true)[0]).Value = MainRewardChance;

                        ((NumericUpDown)this.Controls.Find("R11" + i.ToString(), true)[0]).Value = MRewardData[MainRewardBase + 4];

                        int MainRewardId = BitConverter.ToInt16(MRewardData, MainRewardBase + 2);
                        List.Item.TryGetValue(MainRewardId, out string MainRewardItemName);
                        ((TextBox)this.Controls.Find("R10" + i.ToString(), true)[0]).Text = MainRewardItemName;
                    }

                    if (!((ARewardPointer == 0)))
                    {
                        for (int i = 10; i < 30; i++)
                        {
                            int ARewardBase = (i - 10) * 6;

                            int ARewardChance = ARewardData[ARewardBase];
                            if (ARewardChance == 255)
                            {
                                break;
                            }
                            ((NumericUpDown)this.Controls.Find("R15" + i.ToString(), true)[0]).Value = ARewardChance;

                            ((NumericUpDown)this.Controls.Find("R14" + i.ToString(), true)[0]).Value = ARewardData[ARewardBase + 4];

                            int ARewardId = BitConverter.ToInt16(ARewardData, ARewardBase + 2);
                            List.Item.TryGetValue(ARewardId, out string ARewardItemName);
                            ((TextBox)this.Controls.Find("R13" + i.ToString(), true)[0]).Text = ARewardItemName;
                        }
                    }

                    if (!(BRewardPointer == 0))
                    {
                        for (int i = 10; i < 30; i++)
                        {
                            int BRewardBase = (i - 10) * 6;

                            int BRewardChance = BRewardData[BRewardBase];
                            if (BRewardChance == 255)
                            {
                                break;
                            }
                            ((NumericUpDown)this.Controls.Find("R18" + i.ToString(), true)[0]).Value = BRewardChance;

                            ((NumericUpDown)this.Controls.Find("R17" + i.ToString(), true)[0]).Value = BRewardData[BRewardBase + 4];

                            int BRewardId = BitConverter.ToInt16(BRewardData, BRewardBase + 2);
                            List.Item.TryGetValue(BRewardId, out string BRewardItemName);
                            ((TextBox)this.Controls.Find("R16" + i.ToString(), true)[0]).Text = BRewardItemName;
                        }
                    }

                    if (!(An1RewardPointer == 0))
                    {
                        for (int i = 10; i < 20; i++)
                        {
                            int An1RewardBase = (i - 10) * 6;

                            int An1RewardChance = An1RewardData[An1RewardBase];
                            if (An1RewardChance == 255)
                            {
                                break;
                            }
                            ((NumericUpDown)this.Controls.Find("R21" + i.ToString(), true)[0]).Value = An1RewardChance;

                            ((NumericUpDown)this.Controls.Find("R19" + i.ToString(), true)[0]).Value = An1RewardData[An1RewardBase + 4];

                            int BRewardId = BitConverter.ToInt16(An1RewardData, An1RewardBase + 2);
                            List.Item.TryGetValue(BRewardId, out string An1RewardItemName);
                            ((TextBox)this.Controls.Find("R20" + i.ToString(), true)[0]).Text = An1RewardItemName;
                        }
                    }



                    //Load large monster
                    int MainMonsInfoStart = BitConverter.ToInt16(ba, 24) + 64;
                    bool isEmpty1 = false;
                    bool isEmpty2 = false;
                    bool isEmpty3 = false;
                    bool isEmpty4 = false;

                    int MainMonsID1 = BitConverter.ToInt16(ba, (BitConverter.ToInt16(ba, 24) + 32));
                    if (!(MainMonsID1 == 0))
                    {
                        List.Monster1.TryGetValue(MainMonsID1, out string MainMonsStr1);
                        comboBox_Lar1Boss_Monster.Text = MainMonsStr1;
                        byte[] MainMonsData1 = ba.Skip(MainMonsInfoStart).Take(60).ToArray();
                        comboBox_Lar1Boss_SpawnArea.Items.Add(BitConverter.ToInt16(MainMonsData1, 8).ToString("X2"));
                        comboBox_Lar1Boss_SpawnArea.SelectedIndex = 0;

                        int MainMonsInfoStart1 = BitConverter.ToInt16(ba, 24) + 64;
                        byte[] templi = ba.Skip(MainMonsInfoStart1).Take(60).ToArray();
                        textBox1.Text = BitConverter.ToString(templi).Replace("-", string.Empty);
                    }
                    else { comboBox_Lar1Boss_Monster.Text = "None"; isEmpty1 = true; }

                    int MainMonsID2 = BitConverter.ToInt16(ba, (BitConverter.ToInt16(ba, 24) + 32 + 4));
                    if (!(MainMonsID2 == 0))
                    {
                        List.Monster1.TryGetValue(MainMonsID2, out string MainMonsStr2);
                        comboBox17.Text = MainMonsStr2;
                        byte[] MainMonsData2 = ba.Skip(MainMonsInfoStart + 60).Take(60).ToArray();
                        comboBox18.Items.Add(BitConverter.ToInt16(MainMonsData2, 8).ToString("X2"));
                        comboBox18.SelectedIndex = 0;
                    }
                    else
                    {
                        comboBox17.Text = "None";
                        isEmpty2 = true;
                    }

                    int MainMonsID3 = BitConverter.ToInt16(ba, (BitConverter.ToInt16(ba, 24) + 32 + 8));
                    if (!(MainMonsID3 == 0))
                    {
                        List.Monster1.TryGetValue(MainMonsID3, out string MainMonsStr3);
                        comboBox19.Text = MainMonsStr3;
                        byte[] MainMonsData3 = File.ReadAllBytes(fileloc).Skip(MainMonsInfoStart + 120).Take(60).ToArray();
                        comboBox20.Items.Add(BitConverter.ToInt16(MainMonsData3, 8).ToString("X2"));
                        comboBox20.SelectedIndex = 0;
                    }
                    else { comboBox19.Text = "None"; isEmpty3 = true; }

                    int MainMonsID4 = BitConverter.ToInt16(ba, (BitConverter.ToInt16(ba, 24) + 32 + 12));
                    if (!(MainMonsID4 == 0))
                    {
                        List.Monster1.TryGetValue(MainMonsID4, out string MainMonsStr4);
                        comboBox21.Text = MainMonsStr4;
                        byte[] MainMonsData4 = File.ReadAllBytes(fileloc).Skip(MainMonsInfoStart + 180).Take(60).ToArray();
                        comboBox22.Items.Add(BitConverter.ToInt16(MainMonsData4, 8).ToString("X2"));
                        comboBox22.SelectedIndex = 0;
                    }
                    else { comboBox21.Text = "None"; isEmpty4 = true; }



                    numericUpDown_LarBoss_Strenght.Value = BitConverter.ToInt16(ba, 72);    //str
                    numericUpDown_LarBoss_Size.Value = BitConverter.ToInt16(ba, 68);   //size
                    numericUpDown1_LarBoss_SizeRange.Value = BitConverter.ToInt16(ba, 70);   //size range

                    int MainMonsSpawnArea = BitConverter.ToInt16(ba, (BitConverter.ToInt16(ba, 24) + 72));
                    List.AreaID.TryGetValue(MainMonsSpawnArea, out string MainMonsSpawnArea1);
                    //int index = comboBox12.Items.IndexOf(MainMonsSpawnArea1);
                    //comboBox12.SelectedIndex = index;
                    //int index = comboBox12.Items.IndexOf(MainMonsSpawnArea1);
                    //comboBox12.SelectedIndex = comboBox12.Items.IndexOf(MainMonsSpawnArea1);

                    //Load small monster
                    numericUpDown_SmallMonsterStrength.Value = BitConverter.ToInt16(ba, 97);

                    //Lod HRP
                    numericUpDown_MainHRP.Value = BitConverter.ToInt32(ba, 76);
                    numericUpDown_SubAHRP.Value = BitConverter.ToInt32(ba, 84);
                    numericUpDown_SubBHRP.Value = BitConverter.ToInt32(ba, 88);

                    //Load species
                    switch (ba[337])
                    {
                        case 0:
                            comboBox_LarBoss_Species.SelectedIndex = 0;        //normal
                            break;
                        case 1:
                            comboBox_LarBoss_Species.SelectedIndex = 1;        //HC
                            break;
                        case 2:
                            comboBox_LarBoss_Species.SelectedIndex = 2;        //Geki
                            break;
                        case 9:
                            comboBox_LarBoss_Species.SelectedIndex = 3;        //Hasyu
                            break;
                        case 10:
                            comboBox_LarBoss_Species.SelectedIndex = 4;
                            break;
                        case 16:
                            comboBox_LarBoss_Species.SelectedIndex = 5;
                            break;
                        default:
                            comboBox_LarBoss_Species.SelectedIndex = 6;
                            break;
                    }


                    //Load clear condition
                    if (ba[MH2dosTopoffset + 68] == 2)
                    {
                        comboBox_ClearCondition.SelectedIndex = 0;
                    }
                    else if (ba[MH2dosTopoffset + 68] == 3)
                    {
                        comboBox_ClearCondition.SelectedIndex = 1;
                    }
                    else
                    {
                        comboBox_ClearCondition.SelectedIndex = 2;
                    }

                    //chekc annother target
                    numericUpDown_AnotherTgtNumber.Value = BitConverter.ToInt32(ba, 132);
                    if (BitConverter.ToInt16(ba, 128) == 4)
                    {
                        comboBox_AnotherTargetType.SelectedIndex = 1;
                    }
                    else if (BitConverter.ToInt16(ba, 128) == 5)
                    {
                        comboBox_AnotherTargetType.SelectedIndex = 2;
                    }
                    else
                    {
                        comboBox_AnotherTargetType.SelectedIndex = 0;
                    }


                    //time
                    numericUpDown_Time_Min.Value = BitConverter.ToInt32(ba, MH2dosTopoffset + 28) / 30 / 60;
                    //star
                    //todo 2dosstar
                    numericUpDown_StarLevel.Value = Buffer.GetByte(ba, MH2dosTopoffset + 4);

                    //load area
                    for (int y = 0; y < 1; y++)
                    {
                        int AreaIDPointer = BitConverter.ToInt16(ba, 20);
                        int AreaIDPointer1 = BitConverter.ToInt16(ba, AreaIDPointer);
                        if (AreaIDPointer1 == 0)
                        {
                            break;
                        }

                        for (int i = 0; i < 15; i++)
                        {
                            int AreaIDmulti = i * 16;
                            int AreaID = BitConverter.ToInt16(ba, AreaIDPointer1 + AreaIDmulti);
                            if (AreaID == 0)
                            {
                                break;
                            }
                            comboBox_Lar1Boss_SpawnArea.Items.Add(AreaID.ToString("X2"));
                            comboBox18.Items.Add(AreaID.ToString("X2"));
                            comboBox20.Items.Add(AreaID.ToString("X2"));
                            comboBox22.Items.Add(AreaID.ToString("X2"));
                        }
                    }
                    if (isEmpty1)
                    {
                        comboBox_Lar1Boss_SpawnArea.Items.Add(BitConverter.ToInt16(ba, 544));
                        comboBox_Lar1Boss_SpawnArea.SelectedIndex = 0;
                    }
                    if (isEmpty2)
                    {
                        comboBox18.Items.Add(BitConverter.ToInt16(ba, 544));
                        comboBox18.SelectedIndex = 0;
                    }
                    if (isEmpty3)
                    {
                        comboBox20.Items.Add(BitConverter.ToInt16(ba, 544));
                        comboBox20.SelectedIndex = 0;
                    }
                    if (isEmpty4)
                    {
                        comboBox22.Items.Add(BitConverter.ToInt16(ba, 544));
                        comboBox22.SelectedIndex = 0;
                    }



                    radioButton3.Checked = true;    //is load suc
                    btn_save.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Invalid file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    radioButton3.Checked = false;
                    btn_save.Enabled = false;
                }



            }
        }

        private void Save_Quest()
        {
            List<byte> Endofline = new List<byte>
            {
                     255, 255, 00, 00
            };

            saveFileDialog1.Filter = "questID(*.bin)|*.bin|" + "All files(*.*)|*.*";

            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;                                   //open save file dialog
            }

            string path = saveFileDialog1.FileName;
            string fileloc = openFileDialog1.FileName;

            //byte[] ba = File.ReadAllBytes(fileloc);

            MH2Tools tool = new MH2Tools(TempData);

            ////Title and name
            tool.ModifyStringToBytes(txtBox_TitleAndName.Text, tool.QuestStrings_TitleAndNamePtr);

            ////Main obj
            tool.ModifyStringToBytes(txtBox_MainObjective.Text, tool.QuestStrings_MainoObjPtr);

            ////Sub A
            tool.ModifyStringToBytes(txtBox_SubAObjective.Text, tool.QuestStrings_AObjPtr);

            ////SUb B
            tool.ModifyStringToBytes(txtBox_SubBObjective.Text, tool.QuestStrings_BObjPtr);

            ////Clear
            tool.ModifyStringToBytes(txtBox_ClearCondition.Text, tool.QuestStrings_ClearCPtr);

            ////Fail
            tool.ModifyStringToBytes(txtBox_FailCondition.Text, tool.QuestStrings_FailCPtr);

            ////Emp
            tool.ModifyStringToBytes(txtBox_Employer.Text, tool.QuestStrings_HirerPtr);

            ////Text
            tool.ModifyStringToBytes(txtBox_Description.Text, tool.QuestStrings_TextPtr);


            //Supply
            #region 支给品
            //byte[] eb2 = EntireBytes.ToArray();
            //byte[] NewSplHeader = BitConverter.GetBytes(EntireBytes.Count);
            //指针修正
            //EntireBytes[8] = NewSplHeader[0];
            //EntireBytes[9] = NewSplHeader[1];

            byte[] Temprewardarray = { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 };

            for (int i = 0; i < 40; i++)
            {

                if (i < 10)
                {
                    for (int t = 0; t < 10; t += 1)
                    {
                        int p = t * 4 + 2;
                        Temprewardarray[p] = BitConverter.GetBytes(decimal.ToInt32(((NumericUpDown)this.Controls.Find("numericUpDown10" + t.ToString(), true)[0]).Value))[0];  //amount
                        //EntireBytes[SupplyInfoStart + p] = BitConverter.GetBytes(decimal.ToInt32(((NumericUpDown)this.Controls.Find("numericUpDown10" + t.ToString(), true)[0]).Value))[0];

                        string spstr = ((TextBox)this.Controls.Find("textBox10" + t.ToString(), true)[0]).Text;
                        string spid = List.Item.FirstOrDefault(x => x.Value == spstr).Key.ToString();
                        if (spid.Length == 1)
                        {
                            spid = "00000" + spid;
                        }
                        else if (spid.Length == 2)
                        {
                            spid = "0000" + spid;
                        }
                        else if (spid.Length == 3)
                        {
                            spid = "000" + spid;
                        }
                        else if (spid.Length == 4)
                        {
                            spid = "00" + spid;
                        }
                        else if (spid.Length == 6)
                        {
                            spid = "0" + spid;
                        }
                        byte[] spbyte = BitConverter.GetBytes(Convert.ToInt32(spid));
                        Temprewardarray[p - 2] = spbyte[0];
                        Temprewardarray[p - 1] = spbyte[1];
                    }

                }
                else if (i < 40)
                {
                    for (int t = 10; t < 40; t += 1)
                    {
                        int p = t * 4 + 2;
                        Temprewardarray[p] = BitConverter.GetBytes(decimal.ToInt32(((NumericUpDown)this.Controls.Find("numericUpDown1" + t.ToString(), true)[0]).Value))[0];

                        string spstr = ((TextBox)this.Controls.Find("textBox1" + t.ToString(), true)[0]).Text;
                        string spid = List.Item.FirstOrDefault(x => x.Value == spstr).Key.ToString();
                        if (spid.Length == 1)
                        {
                            spid = "00000" + spid;
                        }
                        else if (spid.Length == 2)
                        {
                            spid = "0000" + spid;
                        }
                        else if (spid.Length == 3)
                        {
                            spid = "000" + spid;
                        }
                        else if (spid.Length == 4)
                        {
                            spid = "00" + spid;
                        }
                        else if (spid.Length == 6)
                        {
                            spid = "0" + spid;
                        }
                        byte[] spbyte = BitConverter.GetBytes(Convert.ToInt32(spid));
                        Temprewardarray[p - 2] = spbyte[0];
                        Temprewardarray[p - 1] = spbyte[1];
                    }
                }
            }
            //EntireBytes.AddRange(Temprewardarray);
            tool.ModifyDataToBytes(Temprewardarray, tool.SuppliesItemPtr);
            #endregion


            //Monster
            #region 怪物设置
            //int MainMonstPointer = BitConverter.ToInt32(ba, 24);
            int MainMonstPointer = tool.BOSSInFoPtr;
            string MonsID1 = List.Monster.FirstOrDefault(x => x.Value == comboBox_Lar1Boss_Monster.Text).Key;
            //eb2[MainMonstPointer + 32] = Convert.ToByte(MonsID1, 16);
            //eb2[MainMonstPointer + 64] = Convert.ToByte(MonsID1, 16);
            tool.ModifyByte(tool.BOSSInFo_Monster1IDPtr, Convert.ToByte(MonsID1, 16));
            tool.ModifyByte(tool.BOSSInFo_Monster1IDPtr + 64, Convert.ToByte(MonsID1, 16));

            //eb2[72] = BitConverter.GetBytes(decimal.ToInt16(numericUpDown_LarBoss_Strenght.Value))[0];      //str
            //eb2[68] = BitConverter.GetBytes(decimal.ToInt16(numericUpDown_LarBoss_Size.Value))[0];     //size
            //eb2[70] = BitConverter.GetBytes(decimal.ToInt16(numericUpDown1_LarBoss_SizeRange.Value))[0];     //size range

            tool.ModifyByte(tool.StandInfo_MonsterPtr, BitConverter.GetBytes(decimal.ToInt16(numericUpDown_LarBoss_Strenght.Value))[0]);
            tool.ModifyByte(tool.StandInfo_MonsterSizePtr, BitConverter.GetBytes(decimal.ToInt16(numericUpDown_LarBoss_Size.Value))[0]);
            tool.ModifyByte(tool.StandInfo_MonsterSizeRangePtr, BitConverter.GetBytes(decimal.ToInt16(numericUpDown1_LarBoss_SizeRange.Value))[0]);

            switch (comboBox_LarBoss_Species.SelectedIndex)
            {
                //case 0:
                //    eb2[337] = 0;
                //    break;
                //case 1:
                //    eb2[337] = 1;
                //    break;
                //case 2:
                //    eb2[337] = 2;
                //    break;
                //case 3:
                //    eb2[337] = 9;
                //    break;
                //case 4:
                //    eb2[337] = 10;
                //    break;
                //case 5:
                //    eb2[337] = 16;
                //    break;
                //case 6:
                //    eb2[337] = 0;
                //    break;
            }
            //eb2[92] = Convert.ToByte(List.Rank.FirstOrDefault(x => x.Value == comboBox_LarBoss_Carve.Text).Key);     //carve rank
            tool.ModifyByte(tool.StandInfo_LarBoss_CarvePtr, Convert.ToByte(List.Rank.FirstOrDefault(x => x.Value == comboBox_LarBoss_Carve.Text).Key));
            //eb2[97] = BitConverter.GetBytes(decimal.ToInt16(numericUpDown_SmallMonsterStrength.Value))[0];     //small
            tool.ModifyByte(tool.StandInfo_SamllMonsterPtr, BitConverter.GetBytes(decimal.ToInt16(numericUpDown_SmallMonsterStrength.Value))[0]);


            byte[] HRP1 = BitConverter.GetBytes(decimal.ToInt32(numericUpDown_MainHRP.Value));
            //eb2[76] = HRP1[0];
            //eb2[77] = HRP1[1];
            //eb2[78] = HRP1[2];
            tool.ModifyByte(tool.StandInfo_MainHRPPtr, HRP1[0]);
            tool.ModifyByte(tool.StandInfo_MainHRPPtr+1, HRP1[1]);
            tool.ModifyByte(tool.StandInfo_MainHRPPtr+2, HRP1[2]);
            byte[] HRP2 = BitConverter.GetBytes(decimal.ToInt32(numericUpDown_SubAHRP.Value));
            //eb2[84] = HRP2[0];
            //eb2[85] = HRP2[1];
            //eb2[86] = HRP2[2];
            tool.ModifyByte(tool.StandInfo_SubAHRPPtr, HRP2[0]);
            tool.ModifyByte(tool.StandInfo_SubAHRPPtr + 1, HRP2[1]);
            tool.ModifyByte(tool.StandInfo_SubAHRPPtr + 2, HRP2[2]);
            byte[] HRP3 = BitConverter.GetBytes(decimal.ToInt32(numericUpDown_SubBHRP.Value));
            //eb2[88] = HRP3[0];
            //eb2[89] = HRP3[1];
            //eb2[90] = HRP3[2];
            tool.ModifyByte(tool.StandInfo_SubBHRPPtr, HRP3[0]);
            tool.ModifyByte(tool.StandInfo_SubBHRPPtr + 1, HRP3[1]);
            tool.ModifyByte(tool.StandInfo_SubBHRPPtr + 2, HRP3[2]);
            #endregion

            #region Lrg mosnter

            List<byte> BossEntireBytes = new List<byte>();
            //int tempnum = EntireBytes.Count;
            int tempnum = tool.BOSSInFoPtr;
            byte[] tempnum1 = BitConverter.GetBytes(Convert.ToInt16(tempnum));
            //指针修正
            //EntireBytes[24] = tempnum1[0];  //replace new header
            //EntireBytes[25] = tempnum1[1];

            byte[] tempLMinfo = { 01, 00, 00, 00, 00, 00, 00, 00, 00, 10, 00, 00, 20, 10, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 };

            //应该是查找第一个Boss的指针
            //tempnum = (EntireBytes.Count) + 32;
            tempnum = (tool.BOSSInFoPtr) + 32;
            tempnum1 = BitConverter.GetBytes(Convert.ToInt16(tempnum));
            tempLMinfo[8] = tempnum1[0];        //replace new header
            tempLMinfo[9] = tempnum1[1];

            tempnum = tempnum + 32;
            tempnum1 = BitConverter.GetBytes(Convert.ToInt16(tempnum));
            tempLMinfo[12] = tempnum1[0];       //replace new header
            tempLMinfo[13] = tempnum1[1];
            BossEntireBytes.AddRange(tempLMinfo);

            string templateMonsterHexData = textBox1.Text;
            var templateMonsterData = new List<byte>();
            for (int i = 0; i < templateMonsterHexData.Length / 2; i++)
            {
                templateMonsterData.Add(Convert.ToByte(templateMonsterHexData.Substring(i * 2, 2), 16));
            }

            byte[] tempLMinfotemo = { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 255, 255, 255, 255, 255, 255, 255, 255, 00, 00, 00, 00 };
            var tempLmdata = new List<byte>();

            int LMons1 = List.Monster1.FirstOrDefault(x => x.Value == comboBox_Lar1Boss_Monster.Text).Key;
            tempLMinfotemo[0] = BitConverter.GetBytes(LMons1)[0];
            byte[] templateMonsterData1 = new byte[60];
            templateMonsterData.CopyTo(templateMonsterData1, 0);
            templateMonsterData1[0] = BitConverter.GetBytes(LMons1)[0];

            string input1 = comboBox_Lar1Boss_SpawnArea.Text;
            var areaid1 = new List<byte>();
            for (int i = 0; i < input1.Length / 2; i++)
            {
                areaid1.Add(Convert.ToByte(input1.Substring(i * 2, 2), 16));
            }


            int LMons2 = List.Monster1.FirstOrDefault(x => x.Value == comboBox17.Text).Key;
            tempLMinfotemo[4] = BitConverter.GetBytes(LMons2)[0];
            byte[] templateMonsterData2 = new byte[60];
            templateMonsterData.CopyTo(templateMonsterData2, 0);
            templateMonsterData2[0] = BitConverter.GetBytes(LMons2)[0];

            string input2 = comboBox18.Text;
            var areaid2 = new List<byte>();
            for (int i = 0; i < input2.Length / 2; i++)
            {
                areaid2.Add(Convert.ToByte(input2.Substring(i * 2, 2), 16));
            }
            templateMonsterData2[8] = areaid2[0];


            int LMons3 = List.Monster1.FirstOrDefault(x => x.Value == comboBox19.Text).Key;
            tempLMinfotemo[8] = BitConverter.GetBytes(LMons3)[0];
            byte[] templateMonsterData3 = new byte[60];
            templateMonsterData.CopyTo(templateMonsterData3, 0);
            templateMonsterData3[0] = BitConverter.GetBytes(LMons3)[0];

            string input3 = comboBox20.Text;
            var areaid3 = new List<byte>();
            for (int i = 0; i < input3.Length / 2; i++)
            {
                areaid3.Add(Convert.ToByte(input3.Substring(i * 2, 2), 16));
            }
            templateMonsterData3[8] = areaid3[0];

            int LMons4 = List.Monster1.FirstOrDefault(x => x.Value == comboBox21.Text).Key;
            tempLMinfotemo[12] = BitConverter.GetBytes(LMons4)[0];
            byte[] templateMonsterData4 = new byte[60];
            templateMonsterData.CopyTo(templateMonsterData4, 0);
            templateMonsterData4[0] = BitConverter.GetBytes(LMons4)[0];

            string input4 = comboBox22.Text;
            var areaid4 = new List<byte>();
            for (int i = 0; i < input4.Length / 2; i++)
            {
                areaid4.Add(Convert.ToByte(input4.Substring(i * 2, 2), 16));
            }
            templateMonsterData4[8] = areaid4[0];


            BossEntireBytes.AddRange(tempLMinfotemo);
            BossEntireBytes.AddRange(templateMonsterData1);
            BossEntireBytes.AddRange(templateMonsterData2);
            BossEntireBytes.AddRange(templateMonsterData3);
            BossEntireBytes.AddRange(templateMonsterData4);
            BossEntireBytes.AddRange(Endofline);

            tool.ModifyDataToBytes(BossEntireBytes.ToArray(), tool.BOSSInFoPtr);
            #endregion

            //Fee
            byte[] feeb = BitConverter.GetBytes(decimal.ToInt32(numericUpDown_fee.Value));
            //eb2[204] = feeb[0];
            //eb2[205] = feeb[1];
            //eb2[206] = feeb[2];
            tool.ModifyByte(tool.QuestInfo_FeePtr, feeb[0]);
            tool.ModifyByte(tool.QuestInfo_FeePtr+1, feeb[1]);
            tool.ModifyByte(tool.QuestInfo_FeePtr+2, feeb[2]);

            //Penalty
            byte[] penalty = BitConverter.GetBytes(decimal.ToInt32(numericUpDown_Penalty.Value));
            //eb2[212] = penalty[0];
            //eb2[213] = penalty[1];
            //eb2[214] = penalty[2];
            tool.ModifyByte(tool.QuestInfo_PenaltyPtr, penalty[0]);
            tool.ModifyByte(tool.QuestInfo_PenaltyPtr + 1, penalty[1]);
            tool.ModifyByte(tool.QuestInfo_PenaltyPtr + 2, penalty[2]);

            //Main
            #region Main
            string mat = List.ObjectiveType.FirstOrDefault(x => x.Value == comboBox_MainType.Text).Key;
            byte[] madata = Enumerable.Range(0, mat.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(mat.Substring(x, 2), 16)).ToArray();
            //eb2[240] = madata[0];
            //eb2[241] = madata[1];
            //eb2[242] = madata[2];
            //eb2[243] = madata[3];
            //居然是4个
            tool.ModifyByte(tool.QuestInfo_MainTypePtr, madata[0]);
            tool.ModifyByte(tool.QuestInfo_MainTypePtr + 1, madata[1]);
            tool.ModifyByte(tool.QuestInfo_MainTypePtr + 2, madata[2]);
            tool.ModifyByte(tool.QuestInfo_MainTypePtr + 3, madata[3]);

            //如果是缴纳
            if (madata[0] == 02 & madata[1] == 00)
            {
                int mot = List.Item.FirstOrDefault(x => x.Value == textBox_MainTarget.Text).Key;
                byte[] motitem = BitConverter.GetBytes(mot);
                //eb2[244] = motitem[0];
                //eb2[245] = motitem[1];
                tool.ModifyByte(tool.QuestInfo_MainTargetIDPtr, motitem[0]);
                tool.ModifyByte(tool.QuestInfo_MainTargetIDPtr + 1, motitem[1]);
            }
            else//击杀
            {
                string mot = List.Monster.FirstOrDefault(x => x.Value == textBox_MainTarget.Text).Key;
                //eb2[244] = Convert.ToByte(mot, 16);
                tool.ModifyByte(tool.QuestInfo_MainTargetIDPtr, Convert.ToByte(mot, 16));
            }

            decimal MainAmount = numericUpDown_MainAmount.Value;
            if (comboBox_MainType.SelectedIndex == 4)
            {
                MainAmount = MainAmount / 100;
            }
            //目标数量
            byte[] mamt = BitConverter.GetBytes(decimal.ToInt32(MainAmount));
            //eb2[246] = mamt[0];
            //eb2[247] = mamt[1];
            tool.ModifyByte(tool.QuestInfo_MainTargetCountPtr, mamt[0]);
            tool.ModifyByte(tool.QuestInfo_MainTargetCountPtr + 1, mamt[1]);

            //报酬金
            byte[] mrwd = BitConverter.GetBytes(decimal.ToInt32(numericUpDown_MainRewardMoney.Value));
            //eb2[208] = mrwd[0];
            //eb2[209] = mrwd[1];
            //eb2[210] = mrwd[2];
            tool.ModifyByte(tool.QuestInfo_RewardMoneyPtr, mrwd[0]);
            tool.ModifyByte(tool.QuestInfo_RewardMoneyPtr + 1, mrwd[1]);
            tool.ModifyByte(tool.QuestInfo_RewardMoneyPtr + 2, mrwd[2]);
            #endregion

            //Sub A
            #region SubA
            string subat = List.ObjectiveType.FirstOrDefault(x => x.Value == comboBox_SubAType.Text).Key;
            if (!(subat == "00000000"))
            {
                byte[] subadata = Enumerable.Range(0, subat.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(subat.Substring(x, 2), 16)).ToArray();
                //eb2[248] = subadata[0];
                //eb2[249] = subadata[1];
                //eb2[250] = subadata[2];
                //eb2[251] = subadata[3];
                tool.ModifyByte(tool.QuestInfo_SubATypePtr, subadata[0]);
                tool.ModifyByte(tool.QuestInfo_SubATypePtr + 1, subadata[1]);
                tool.ModifyByte(tool.QuestInfo_SubATypePtr + 2, subadata[2]);
                tool.ModifyByte(tool.QuestInfo_SubATypePtr + 3, subadata[4]);

                if (subadata[0] == 02 & subadata[1] == 00)
                {
                    int subatt = List.Item.FirstOrDefault(x => x.Value == textBox_SubATarget.Text).Key;
                    byte[] subaitem = BitConverter.GetBytes(subatt);
                    //eb2[252] = subaitem[0];
                    //eb2[253] = subaitem[1];
                    tool.ModifyByte(tool.QuestInfo_SubATargetIDPtr, subaitem[0]);
                    tool.ModifyByte(tool.QuestInfo_SubATargetIDPtr + 1, subaitem[1]);
                }
                else
                {
                    string subatt = List.Monster.FirstOrDefault(x => x.Value == textBox_SubATarget.Text).Key;
                    //eb2[252] = Convert.ToByte(subatt, 16);
                    //eb2[253] = 0;
                    tool.ModifyByte(tool.QuestInfo_SubATargetIDPtr, Convert.ToByte(subatt, 16));
                    tool.ModifyByte(tool.QuestInfo_SubATargetIDPtr + 1, 0);
                }

                decimal AAmount = numericUpDown_SubA_Amount.Value;
                if (comboBox_SubAType.SelectedIndex == 4)
                {
                    AAmount = AAmount / 100;
                }
                byte[] subaamt = BitConverter.GetBytes(decimal.ToInt32(AAmount));
                //eb2[254] = subaamt[0];
                //eb2[255] = subaamt[1];
                tool.ModifyByte(tool.QuestInfo_SubATargetCountPtr, subaamt[0]);
                tool.ModifyByte(tool.QuestInfo_SubATargetCountPtr + 1, subaamt[1]);

                byte[] subatrd = BitConverter.GetBytes(decimal.ToInt32(numericUpDown_SubARewardMoney.Value));
                //eb2[216] = subatrd[0];
                //eb2[217] = subatrd[1];
                //eb2[218] = subatrd[2];
                tool.ModifyByte(tool.QuestInfo_SubARewardMoneyPtr, subatrd[0]);
                tool.ModifyByte(tool.QuestInfo_SubARewardMoneyPtr + 1, subatrd[1]);
                tool.ModifyByte(tool.QuestInfo_SubARewardMoneyPtr + 2, subatrd[2]);
            }
            #endregion

            //Sub B
            #region SubB
            string subbt = List.ObjectiveType.FirstOrDefault(x => x.Value == comboBox_SubBType.Text).Key;
            if (!(subbt == "00000000"))
            {
                byte[] subbdata = Enumerable.Range(0, subbt.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(subbt.Substring(x, 2), 16)).ToArray();
                //eb2[256] = subbdata[0];
                //eb2[257] = subbdata[1];
                //eb2[258] = subbdata[2];
                //eb2[259] = subbdata[3];
                tool.ModifyByte(tool.QuestInfo_SubBTypePtr, subbdata[0]);
                tool.ModifyByte(tool.QuestInfo_SubBTypePtr + 1, subbdata[1]);
                tool.ModifyByte(tool.QuestInfo_SubBTypePtr + 2, subbdata[2]);
                tool.ModifyByte(tool.QuestInfo_SubBTypePtr + 3, subbdata[4]);

                if (subbdata[0] == 02 & subbdata[1] == 00)
                {
                    int subbtt = List.Item.FirstOrDefault(x => x.Value == textBox_SubBTarget.Text).Key;
                    byte[] subbitem = BitConverter.GetBytes(subbtt);
                    //eb2[260] = subbitem[0];
                    //eb2[261] = subbitem[1];
                    tool.ModifyByte(tool.QuestInfo_SubBTargetIDPtr, subbitem[0]);
                    tool.ModifyByte(tool.QuestInfo_SubBTargetIDPtr + 3, subbitem[1]);
                }
                else
                {
                    string subbtt = List.Monster.FirstOrDefault(x => x.Value == textBox_SubBTarget.Text).Key;
                    //eb2[260] = Convert.ToByte(subbtt, 16);
                    //eb2[261] = 0;
                    tool.ModifyByte(tool.QuestInfo_SubATargetIDPtr, Convert.ToByte(subbtt, 16));
                    tool.ModifyByte(tool.QuestInfo_SubATargetIDPtr + 1, 0);
                }

                decimal BAmount = numericUpDown_SubB_Amount.Value;
                if (comboBox_SubBType.SelectedIndex == 4)
                {
                    BAmount = BAmount / 100;
                }
                byte[] subbamt = BitConverter.GetBytes(decimal.ToInt32(BAmount));
                //eb2[262] = subbamt[0];
                //eb2[263] = subbamt[1];
                tool.ModifyByte(tool.QuestInfo_SubBTargetCountPtr, subbamt[0]);
                tool.ModifyByte(tool.QuestInfo_SubBTargetCountPtr + 1, subbamt[1]);

                byte[] subbtrd = BitConverter.GetBytes(decimal.ToInt32(numericUpDown_SubBRewardMoney.Value));
                //eb2[220] = subbtrd[0];
                //eb2[221] = subbtrd[1];
                //eb2[222] = subbtrd[2];
                tool.ModifyByte(tool.QuestInfo_SubBRewardMoneyPtr, subbtrd[0]);
                tool.ModifyByte(tool.QuestInfo_SubBRewardMoneyPtr + 1, subbtrd[1]);
                tool.ModifyByte(tool.QuestInfo_SubBRewardMoneyPtr + 2, subbtrd[2]);
            }
            #endregion


            //Clear condition
            #region Clear condition
            //if (comboBox_ClearCondition.SelectedIndex == 0)
            //{
            //    eb2[264] = 2;
            //}
            //else if (comboBox_ClearCondition.SelectedIndex == 1)
            //{
            //    eb2[264] = 3;
            //}
            //else
            //{
            //    eb2[264] = 4;
            //}
            #endregion

            //Another target
            #region Another target
            //TODO
            //if (!(numericUpDown_AnotherTgtNumber.Value == 0))
            //{
            //    eb2[128] = BitConverter.GetBytes(comboBox_AnotherTargetType.SelectedIndex + 3)[0];
            //    eb2[130] = Convert.ToByte(MonsID1, 16);
            //    eb2[132] = BitConverter.GetBytes(decimal.ToInt16(numericUpDown_AnotherTgtNumber.Value))[0];
            //}
            //else
            //{
            //    eb2[128] = 0;
            //    eb2[130] = 0;
            //    eb2[132] = 0;
            //}

            byte[] QuestTime = BitConverter.GetBytes(decimal.ToInt32(numericUpDown_Time_Min.Value * 30 * 60));     //time
            //eb2[224] = QuestTime[0];
            //eb2[225] = QuestTime[1];
            //eb2[226] = QuestTime[2];
            //eb2[227] = QuestTime[3];
            tool.ModifyByte(tool.QuestInfo_TimePtr, QuestTime[0]);
            tool.ModifyByte(tool.QuestInfo_TimePtr + 1, QuestTime[1]);
            tool.ModifyByte(tool.QuestInfo_TimePtr + 2, QuestTime[0]);
            tool.ModifyByte(tool.QuestInfo_TimePtr + 3, QuestTime[1]);

            byte[] NumStar = BitConverter.GetBytes(decimal.ToInt16(numericUpDown_StarLevel.Value));     //difficulty
            //eb2[196] = NumStar[0];
            //eb2[197] = NumStar[1];
            tool.ModifyByte(tool.QuestInfo_StarPtr, NumStar[0]);
            tool.ModifyByte(tool.QuestInfo_StarPtr + 1, penalty[1]);
            #endregion
            File.WriteAllBytes(path, tool.GetCurrData());
        }

        private void btn_save_Click_src(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "questID(*.bin)|*.bin|" + "All files(*.*)|*.*";

            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;                                   //open save file dialog
            }

            string path = saveFileDialog1.FileName;
            string fileloc = openFileDialog1.FileName;

            byte[] ba = File.ReadAllBytes(fileloc);

            int MH2dosTopoffset = (BitConverter.ToInt32(ba, 0));

            int questStringsStart = BitConverter.ToInt32(ba, MH2dosTopoffset + 36);
            //int questStringsStart = BitConverter.ToInt32(ba, 48);
            int readPointer = BitConverter.ToInt32(ba, questStringsStart);
            int pTitleAndName = BitConverter.ToInt32(ba, (readPointer - 32));
            int RewardHeaderStart = BitConverter.ToInt32(ba, 12);
            int SupplyInfoStart = BitConverter.ToInt32(ba, 8);

            string HexData = textBox32.Text;        //get entire text string
            var EntireBytes = new List<byte>();          //create empty byte list
            for (int i = 0; i < HexData.Length / 2; i++)
            {
                EntireBytes.Add(Convert.ToByte(HexData.Substring(i * 2, 2), 16));        //convert string to byte list
            }
            EntireBytes.RemoveRange(pTitleAndName, EntireBytes.Count - pTitleAndName);        //Remove all text data to add new one
            //取临时数据

            //Write
            List<byte> divider = new List<byte>
            {
                00
            };

            ////Title and name
            EntireBytes.AddRange(Encoding.GetEncoding("Shift_JIS").GetBytes(txtBox_TitleAndName.Text.Replace("\r\n", "\n")).ToList());
            EntireBytes.AddRange(divider);

            ////Main obj
            byte[] mo = BitConverter.GetBytes(EntireBytes.Count);
            EntireBytes.AddRange(Encoding.GetEncoding("Shift_JIS").GetBytes(txtBox_MainObjective.Text.Replace("\r\n", "\n")).ToList());
            EntireBytes.AddRange(divider);

            ////Sub A
            byte[] sa = BitConverter.GetBytes(EntireBytes.Count);
            EntireBytes.AddRange(Encoding.GetEncoding("Shift_JIS").GetBytes(txtBox_SubAObjective.Text.Replace("\r\n", "\n")).ToList());
            EntireBytes.AddRange(divider);

            ////SUb B
            byte[] sb = BitConverter.GetBytes(EntireBytes.Count);
            EntireBytes.AddRange(Encoding.GetEncoding("Shift_JIS").GetBytes(txtBox_SubBObjective.Text.Replace("\r\n", "\n")).ToList());
            EntireBytes.AddRange(divider);

            ////Clear
            byte[] cc = BitConverter.GetBytes(EntireBytes.Count);
            EntireBytes.AddRange(Encoding.GetEncoding("Shift_JIS").GetBytes(txtBox_ClearCondition.Text.Replace("\r\n", "\n")).ToList());
            EntireBytes.AddRange(divider);

            ////Fail
            byte[] fc = BitConverter.GetBytes(EntireBytes.Count);
            EntireBytes.AddRange(Encoding.GetEncoding("Shift_JIS").GetBytes(txtBox_FailCondition.Text.Replace("\r\n", "\n")).ToList());
            EntireBytes.AddRange(divider);

            ////Emp
            byte[] em = BitConverter.GetBytes(EntireBytes.Count);
            EntireBytes.AddRange(Encoding.GetEncoding("Shift_JIS").GetBytes(txtBox_Employer.Text.Replace("\r\n", "\n")).ToList());
            EntireBytes.AddRange(divider);

            ////Text
            byte[] tx = BitConverter.GetBytes(EntireBytes.Count);
            EntireBytes.AddRange(Encoding.GetEncoding("Shift_JIS").GetBytes(txtBox_Description.Text.Replace("\r\n", "\n")).ToList());
            EntireBytes.AddRange(divider);

            //Reward
            #region Reward
            List<byte> Endofline = new List<byte>
            {
                     255, 255, 00, 00
            };

            int HaveA = comboBox_SubAType.SelectedIndex;
            int HaveB = comboBox_SubBType.SelectedIndex;
            decimal HaveAnt = numericUpDown_AnotherTgtNumber.Value;
            byte[] NewRewardHeader = BitConverter.GetBytes(EntireBytes.Count);      //get entire length and convert to byte[]
            EntireBytes[12] = NewRewardHeader[0];
            EntireBytes[13] = NewRewardHeader[1];

            if ((!(HaveB == 0)) & (HaveAnt == 0))       //Main A B
            {
                List<byte> HeaderMainAB = new List<byte>
                {01, 128, 00, 00, 255, 255, 00, 00, 02, 128, 00, 00, 255, 255, 00, 00, 03, 128, 00, 00, 255, 255, 00, 00, 255, 255, 00, 00, 00, 00, 00, 00};
                EntireBytes.AddRange(HeaderMainAB);
                int NewRewardHeader1 = EntireBytes.Count - 32;

                EntireBytes[NewRewardHeader1 + 4] = BitConverter.GetBytes(EntireBytes.Count)[0];    //replace with new main header
                EntireBytes[NewRewardHeader1 + 5] = BitConverter.GetBytes(EntireBytes.Count)[1];
                List<byte> NewMainRewardData = new List<byte> { };
                for (int i = 10; i < 50; i++)
                {
                    byte[] MChance = BitConverter.GetBytes(decimal.ToInt16(((NumericUpDown)this.Controls.Find("R12" + i.ToString(), true)[0]).Value));
                    NewMainRewardData.AddRange(MChance);
                    //if(MChance[0] == 0) { break; }
                    string NewMainRewardName = ((TextBox)this.Controls.Find("R10" + i.ToString(), true)[0]).Text;
                    NewMainRewardData.AddRange(BitConverter.GetBytes(decimal.ToInt16(List.Item.FirstOrDefault(x => x.Value == NewMainRewardName).Key)));
                    NewMainRewardData.AddRange(BitConverter.GetBytes(decimal.ToInt16(((NumericUpDown)this.Controls.Find("R11" + i.ToString(), true)[0]).Value)));
                }
                NewMainRewardData.AddRange(Endofline);
                EntireBytes.AddRange(NewMainRewardData);


                EntireBytes[NewRewardHeader1 + 12] = BitConverter.GetBytes(EntireBytes.Count)[0];    //replace with new a header
                EntireBytes[NewRewardHeader1 + 13] = BitConverter.GetBytes(EntireBytes.Count)[1];

                List<byte> NewARewardData = new List<byte> { };
                for (int i = 10; i < 30; i++)
                {
                    byte[] AChance = BitConverter.GetBytes(decimal.ToInt16(((NumericUpDown)this.Controls.Find("R15" + i.ToString(), true)[0]).Value));
                    NewARewardData.AddRange(AChance);
                    //if (AChance[0] == 0) { break; }
                    string NewARewardName = ((TextBox)this.Controls.Find("R13" + i.ToString(), true)[0]).Text;
                    NewARewardData.AddRange(BitConverter.GetBytes(decimal.ToInt16(List.Item.FirstOrDefault(x => x.Value == NewARewardName).Key)));
                    NewARewardData.AddRange(BitConverter.GetBytes(decimal.ToInt16(((NumericUpDown)this.Controls.Find("R14" + i.ToString(), true)[0]).Value)));
                }
                NewARewardData.AddRange(Endofline);
                EntireBytes.AddRange(NewARewardData);


                EntireBytes[NewRewardHeader1 + 20] = BitConverter.GetBytes(EntireBytes.Count)[0];    //replace with new B header
                EntireBytes[NewRewardHeader1 + 21] = BitConverter.GetBytes(EntireBytes.Count)[1];
                List<byte> NewBRewardData = new List<byte> { };
                for (int i = 10; i < 30; i++)
                {
                    byte[] BChance = BitConverter.GetBytes(decimal.ToInt16(((NumericUpDown)this.Controls.Find("R18" + i.ToString(), true)[0]).Value));
                    NewBRewardData.AddRange(BChance);
                    //if (BChance[0] == 0) { break; }
                    string NewBRewardName = ((TextBox)this.Controls.Find("R16" + i.ToString(), true)[0]).Text;
                    NewBRewardData.AddRange(BitConverter.GetBytes(decimal.ToInt16(List.Item.FirstOrDefault(x => x.Value == NewBRewardName).Key)));
                    NewBRewardData.AddRange(BitConverter.GetBytes(decimal.ToInt16(((NumericUpDown)this.Controls.Find("R17" + i.ToString(), true)[0]).Value)));
                }
                NewBRewardData.AddRange(Endofline);
                EntireBytes.AddRange(NewBRewardData);
            }
            else if ((!(HaveA == 0)) & (HaveAnt == 0))  //Main A
            {
                List<byte> HeaderMainA = new List<byte>
                {01, 128, 00, 00, 255, 255, 00, 00, 02, 128, 00, 00, 255, 255, 00, 00, 255, 255, 00, 00, 00, 00, 00, 00};
                EntireBytes.AddRange(HeaderMainA);
                int NewRewardHeader1 = EntireBytes.Count - 24;

                EntireBytes[NewRewardHeader1 + 4] = BitConverter.GetBytes(EntireBytes.Count)[0];    //replace with new main header
                EntireBytes[NewRewardHeader1 + 5] = BitConverter.GetBytes(EntireBytes.Count)[1];
                List<byte> NewMainRewardData = new List<byte> { };
                for (int i = 10; i < 50; i++)
                {
                    byte[] MChance = BitConverter.GetBytes(decimal.ToInt16(((NumericUpDown)this.Controls.Find("R12" + i.ToString(), true)[0]).Value));
                    NewMainRewardData.AddRange(MChance);
                    //if(MChance[0] == 0) { break; }
                    string NewMainRewardName = ((TextBox)this.Controls.Find("R10" + i.ToString(), true)[0]).Text;
                    NewMainRewardData.AddRange(BitConverter.GetBytes(decimal.ToInt16(List.Item.FirstOrDefault(x => x.Value == NewMainRewardName).Key)));
                    NewMainRewardData.AddRange(BitConverter.GetBytes(decimal.ToInt16(((NumericUpDown)this.Controls.Find("R11" + i.ToString(), true)[0]).Value)));
                }
                NewMainRewardData.AddRange(Endofline);
                EntireBytes.AddRange(NewMainRewardData);

                EntireBytes[NewRewardHeader1 + 12] = BitConverter.GetBytes(EntireBytes.Count)[0];    //replace with new a header
                EntireBytes[NewRewardHeader1 + 13] = BitConverter.GetBytes(EntireBytes.Count)[1];

                List<byte> NewARewardData = new List<byte> { };
                for (int i = 10; i < 30; i++)
                {
                    byte[] AChance = BitConverter.GetBytes(decimal.ToInt16(((NumericUpDown)this.Controls.Find("R15" + i.ToString(), true)[0]).Value));
                    NewARewardData.AddRange(AChance);
                    //if (AChance[0] == 0) { break; }
                    string NewARewardName = ((TextBox)this.Controls.Find("R13" + i.ToString(), true)[0]).Text;
                    NewARewardData.AddRange(BitConverter.GetBytes(decimal.ToInt16(List.Item.FirstOrDefault(x => x.Value == NewARewardName).Key)));
                    NewARewardData.AddRange(BitConverter.GetBytes(decimal.ToInt16(((NumericUpDown)this.Controls.Find("R14" + i.ToString(), true)[0]).Value)));
                }
                NewARewardData.AddRange(Endofline);
                EntireBytes.AddRange(NewARewardData);
            }
            else if (HaveAnt == 0)      //Main
            {
                List<byte> HeaderMain = new List<byte>
                {01, 128, 00, 00, 255, 255, 00, 00, 255, 255, 00, 00, 00, 00, 00, 00};
                EntireBytes.AddRange(HeaderMain);
                int NewRewardHeader1 = EntireBytes.Count - 16;

                EntireBytes[NewRewardHeader1 + 4] = BitConverter.GetBytes(EntireBytes.Count)[0];    //replace with new main header
                EntireBytes[NewRewardHeader1 + 5] = BitConverter.GetBytes(EntireBytes.Count)[1];
                List<byte> NewMainRewardData = new List<byte> { };
                for (int i = 10; i < 50; i++)
                {
                    byte[] MChance = BitConverter.GetBytes(decimal.ToInt16(((NumericUpDown)this.Controls.Find("R12" + i.ToString(), true)[0]).Value));
                    NewMainRewardData.AddRange(MChance);
                    //if(MChance[0] == 0) { break; }
                    string NewMainRewardName = ((TextBox)this.Controls.Find("R10" + i.ToString(), true)[0]).Text;
                    NewMainRewardData.AddRange(BitConverter.GetBytes(decimal.ToInt16(List.Item.FirstOrDefault(x => x.Value == NewMainRewardName).Key)));
                    NewMainRewardData.AddRange(BitConverter.GetBytes(decimal.ToInt16(((NumericUpDown)this.Controls.Find("R11" + i.ToString(), true)[0]).Value)));
                }
                NewMainRewardData.AddRange(Endofline);
                EntireBytes.AddRange(NewMainRewardData);
            }
            else if ((!(HaveB == 0)) & (!(HaveAnt == 0)))       //Main A B Ant
            {
                List<byte> HeaderMainABAnt = new List<byte>
                {01,128,00,00,255,255,00,00,02,128,00,00,255,255,00,00,03,128,00,00,255,255,00,00,04,128,00,00,255,255,00,00,05,128,00,00,255,255,00,00,255,255,00,00,00,00,00,00};
                EntireBytes.AddRange(HeaderMainABAnt);
                int NewRewardHeader1 = EntireBytes.Count - 48;

                EntireBytes[NewRewardHeader1 + 28] = BitConverter.GetBytes(EntireBytes.Count)[0];    //Ant1
                EntireBytes[NewRewardHeader1 + 29] = BitConverter.GetBytes(EntireBytes.Count)[1];
                List<byte> NewAnt1RewardData = new List<byte> { };
                for (int i = 10; i < 20; i++)
                {
                    byte[] Ant1Chance = BitConverter.GetBytes(decimal.ToInt16(((NumericUpDown)this.Controls.Find("R21" + i.ToString(), true)[0]).Value));
                    NewAnt1RewardData.AddRange(Ant1Chance);
                    string NewAnt1RewardName = ((TextBox)this.Controls.Find("R20" + i.ToString(), true)[0]).Text;
                    NewAnt1RewardData.AddRange(BitConverter.GetBytes(decimal.ToInt16(List.Item.FirstOrDefault(x => x.Value == NewAnt1RewardName).Key)));
                    NewAnt1RewardData.AddRange(BitConverter.GetBytes(decimal.ToInt16(((NumericUpDown)this.Controls.Find("R19" + i.ToString(), true)[0]).Value)));
                }
                NewAnt1RewardData.AddRange(Endofline);
                EntireBytes.AddRange(NewAnt1RewardData);

                EntireBytes[NewRewardHeader1 + 36] = BitConverter.GetBytes(EntireBytes.Count)[0];    //Ant2
                EntireBytes[NewRewardHeader1 + 37] = BitConverter.GetBytes(EntireBytes.Count)[1];
                EntireBytes.AddRange(NewAnt1RewardData);

                EntireBytes[NewRewardHeader1 + 4] = BitConverter.GetBytes(EntireBytes.Count)[0];    //Main
                EntireBytes[NewRewardHeader1 + 5] = BitConverter.GetBytes(EntireBytes.Count)[1];
                List<byte> NewMainRewardData = new List<byte> { };
                for (int i = 10; i < 50; i++)
                {
                    byte[] MChance = BitConverter.GetBytes(decimal.ToInt16(((NumericUpDown)this.Controls.Find("R12" + i.ToString(), true)[0]).Value));
                    NewMainRewardData.AddRange(MChance);
                    //if(MChance[0] == 0) { break; }
                    string NewMainRewardName = ((TextBox)this.Controls.Find("R10" + i.ToString(), true)[0]).Text;
                    NewMainRewardData.AddRange(BitConverter.GetBytes(decimal.ToInt16(List.Item.FirstOrDefault(x => x.Value == NewMainRewardName).Key)));
                    NewMainRewardData.AddRange(BitConverter.GetBytes(decimal.ToInt16(((NumericUpDown)this.Controls.Find("R11" + i.ToString(), true)[0]).Value)));
                }
                NewMainRewardData.AddRange(Endofline);
                EntireBytes.AddRange(NewMainRewardData);


                EntireBytes[NewRewardHeader1 + 12] = BitConverter.GetBytes(EntireBytes.Count)[0];    //replace with new a header
                EntireBytes[NewRewardHeader1 + 13] = BitConverter.GetBytes(EntireBytes.Count)[1];

                List<byte> NewARewardData = new List<byte> { };
                for (int i = 10; i < 30; i++)
                {
                    byte[] AChance = BitConverter.GetBytes(decimal.ToInt16(((NumericUpDown)this.Controls.Find("R15" + i.ToString(), true)[0]).Value));
                    NewARewardData.AddRange(AChance);
                    //if (AChance[0] == 0) { break; }
                    string NewARewardName = ((TextBox)this.Controls.Find("R13" + i.ToString(), true)[0]).Text;
                    NewARewardData.AddRange(BitConverter.GetBytes(decimal.ToInt16(List.Item.FirstOrDefault(x => x.Value == NewARewardName).Key)));
                    NewARewardData.AddRange(BitConverter.GetBytes(decimal.ToInt16(((NumericUpDown)this.Controls.Find("R14" + i.ToString(), true)[0]).Value)));
                }
                NewARewardData.AddRange(Endofline);
                EntireBytes.AddRange(NewARewardData);


                EntireBytes[NewRewardHeader1 + 20] = BitConverter.GetBytes(EntireBytes.Count)[0];    //replace with new B header
                EntireBytes[NewRewardHeader1 + 21] = BitConverter.GetBytes(EntireBytes.Count)[1];
                List<byte> NewBRewardData = new List<byte> { };
                for (int i = 10; i < 30; i++)
                {
                    byte[] BChance = BitConverter.GetBytes(decimal.ToInt16(((NumericUpDown)this.Controls.Find("R18" + i.ToString(), true)[0]).Value));
                    NewBRewardData.AddRange(BChance);
                    //if (BChance[0] == 0) { break; }
                    string NewBRewardName = ((TextBox)this.Controls.Find("R16" + i.ToString(), true)[0]).Text;
                    NewBRewardData.AddRange(BitConverter.GetBytes(decimal.ToInt16(List.Item.FirstOrDefault(x => x.Value == NewBRewardName).Key)));
                    NewBRewardData.AddRange(BitConverter.GetBytes(decimal.ToInt16(((NumericUpDown)this.Controls.Find("R17" + i.ToString(), true)[0]).Value)));
                }
                NewBRewardData.AddRange(Endofline);
                EntireBytes.AddRange(NewBRewardData);
            }
            else if ((!(HaveA == 0)) & (!(HaveAnt == 0)))       //Main A Ant
            {
                List<byte> HeaderMainAAnt = new List<byte>
                {01,128,00,00,255,255,00,00,02,128,00,00,255,255,00,00,04,128,00,00,255,255,00,00,05,128,00,00,255,255,00,00,255,255,00,00,00,00,00,00};
                EntireBytes.AddRange(HeaderMainAAnt);
                int NewRewardHeader1 = EntireBytes.Count - 40;

                EntireBytes[NewRewardHeader1 + 20] = BitConverter.GetBytes(EntireBytes.Count)[0];    //Ant1
                EntireBytes[NewRewardHeader1 + 21] = BitConverter.GetBytes(EntireBytes.Count)[1];
                List<byte> NewAnt2RewardData = new List<byte> { };
                for (int i = 10; i < 20; i++)
                {
                    byte[] Ant2Chance = BitConverter.GetBytes(decimal.ToInt16(((NumericUpDown)this.Controls.Find("R21" + i.ToString(), true)[0]).Value));
                    NewAnt2RewardData.AddRange(Ant2Chance);
                    string NewAnt1RewardName = ((TextBox)this.Controls.Find("R20" + i.ToString(), true)[0]).Text;
                    NewAnt2RewardData.AddRange(BitConverter.GetBytes(decimal.ToInt16(List.Item.FirstOrDefault(x => x.Value == NewAnt1RewardName).Key)));
                    NewAnt2RewardData.AddRange(BitConverter.GetBytes(decimal.ToInt16(((NumericUpDown)this.Controls.Find("R19" + i.ToString(), true)[0]).Value)));
                }
                NewAnt2RewardData.AddRange(Endofline);
                EntireBytes.AddRange(NewAnt2RewardData);

                EntireBytes[NewRewardHeader1 + 28] = BitConverter.GetBytes(EntireBytes.Count)[0];    //Ant2
                EntireBytes[NewRewardHeader1 + 29] = BitConverter.GetBytes(EntireBytes.Count)[1];
                EntireBytes.AddRange(NewAnt2RewardData);

                EntireBytes[NewRewardHeader1 + 4] = BitConverter.GetBytes(EntireBytes.Count)[0];    //Main
                EntireBytes[NewRewardHeader1 + 5] = BitConverter.GetBytes(EntireBytes.Count)[1];
                List<byte> NewMainRewardData = new List<byte> { };
                for (int i = 10; i < 50; i++)
                {
                    byte[] MChance = BitConverter.GetBytes(decimal.ToInt16(((NumericUpDown)this.Controls.Find("R12" + i.ToString(), true)[0]).Value));
                    NewMainRewardData.AddRange(MChance);
                    //if(MChance[0] == 0) { break; }
                    string NewMainRewardName = ((TextBox)this.Controls.Find("R10" + i.ToString(), true)[0]).Text;
                    NewMainRewardData.AddRange(BitConverter.GetBytes(decimal.ToInt16(List.Item.FirstOrDefault(x => x.Value == NewMainRewardName).Key)));
                    NewMainRewardData.AddRange(BitConverter.GetBytes(decimal.ToInt16(((NumericUpDown)this.Controls.Find("R11" + i.ToString(), true)[0]).Value)));
                }
                NewMainRewardData.AddRange(Endofline);
                EntireBytes.AddRange(NewMainRewardData);


                EntireBytes[NewRewardHeader1 + 12] = BitConverter.GetBytes(EntireBytes.Count)[0];    //A
                EntireBytes[NewRewardHeader1 + 13] = BitConverter.GetBytes(EntireBytes.Count)[1];
                List<byte> NewARewardData = new List<byte> { };
                for (int i = 10; i < 30; i++)
                {
                    byte[] AChance = BitConverter.GetBytes(decimal.ToInt16(((NumericUpDown)this.Controls.Find("R15" + i.ToString(), true)[0]).Value));
                    NewARewardData.AddRange(AChance);
                    //if (AChance[0] == 0) { break; }
                    string NewARewardName = ((TextBox)this.Controls.Find("R13" + i.ToString(), true)[0]).Text;
                    NewARewardData.AddRange(BitConverter.GetBytes(decimal.ToInt16(List.Item.FirstOrDefault(x => x.Value == NewARewardName).Key)));
                    NewARewardData.AddRange(BitConverter.GetBytes(decimal.ToInt16(((NumericUpDown)this.Controls.Find("R14" + i.ToString(), true)[0]).Value)));
                }
                NewARewardData.AddRange(Endofline);
                EntireBytes.AddRange(NewARewardData);
            }
            else if (!(HaveAnt == 0))       //Main Ant
            {
                List<byte> HeaderMainAnt = new List<byte>
                {01,128,00,00,255,255,00,00,04,128,00,00,255,255,00,00,05,128,00,00,255,255,00,00,255,255,00,00,00,00,00,00};
                EntireBytes.AddRange(HeaderMainAnt);
                int NewRewardHeader1 = EntireBytes.Count - 32;

                EntireBytes[NewRewardHeader1 + 12] = BitConverter.GetBytes(EntireBytes.Count)[0];    //Ant1
                EntireBytes[NewRewardHeader1 + 13] = BitConverter.GetBytes(EntireBytes.Count)[1];
                List<byte> NewAnt3RewardData = new List<byte> { };
                for (int i = 10; i < 20; i++)
                {
                    byte[] Ant3Chance = BitConverter.GetBytes(decimal.ToInt16(((NumericUpDown)this.Controls.Find("R21" + i.ToString(), true)[0]).Value));
                    NewAnt3RewardData.AddRange(Ant3Chance);
                    string NewAnt1RewardName = ((TextBox)this.Controls.Find("R20" + i.ToString(), true)[0]).Text;
                    NewAnt3RewardData.AddRange(BitConverter.GetBytes(decimal.ToInt16(List.Item.FirstOrDefault(x => x.Value == NewAnt1RewardName).Key)));
                    NewAnt3RewardData.AddRange(BitConverter.GetBytes(decimal.ToInt16(((NumericUpDown)this.Controls.Find("R19" + i.ToString(), true)[0]).Value)));
                }
                NewAnt3RewardData.AddRange(Endofline);
                EntireBytes.AddRange(NewAnt3RewardData);

                EntireBytes[NewRewardHeader1 + 20] = BitConverter.GetBytes(EntireBytes.Count)[0];    //Ant2
                EntireBytes[NewRewardHeader1 + 21] = BitConverter.GetBytes(EntireBytes.Count)[1];
                EntireBytes.AddRange(NewAnt3RewardData);

                EntireBytes[NewRewardHeader1 + 4] = BitConverter.GetBytes(EntireBytes.Count)[0];    //Main
                EntireBytes[NewRewardHeader1 + 5] = BitConverter.GetBytes(EntireBytes.Count)[1];
                List<byte> NewMainRewardData = new List<byte> { };
                for (int i = 10; i < 50; i++)
                {
                    byte[] MChance = BitConverter.GetBytes(decimal.ToInt16(((NumericUpDown)this.Controls.Find("R12" + i.ToString(), true)[0]).Value));
                    NewMainRewardData.AddRange(MChance);
                    //if(MChance[0] == 0) { break; }
                    string NewMainRewardName = ((TextBox)this.Controls.Find("R10" + i.ToString(), true)[0]).Text;
                    NewMainRewardData.AddRange(BitConverter.GetBytes(decimal.ToInt16(List.Item.FirstOrDefault(x => x.Value == NewMainRewardName).Key)));
                    NewMainRewardData.AddRange(BitConverter.GetBytes(decimal.ToInt16(((NumericUpDown)this.Controls.Find("R11" + i.ToString(), true)[0]).Value)));
                }
                NewMainRewardData.AddRange(Endofline);
                EntireBytes.AddRange(NewMainRewardData);
            }
            #endregion
            //Supply
            #region Supply
            byte[] eb2 = EntireBytes.ToArray();
            byte[] NewSplHeader = BitConverter.GetBytes(EntireBytes.Count);
            EntireBytes[8] = NewSplHeader[0];
            EntireBytes[9] = NewSplHeader[1];

            byte[] Temprewardarray = { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 };

            for (int i = 0; i < 40; i++)
            {

                if (i < 10)
                {
                    for (int t = 0; t < 10; t += 1)
                    {
                        int p = t * 4 + 2;
                        Temprewardarray[p] = BitConverter.GetBytes(decimal.ToInt32(((NumericUpDown)this.Controls.Find("numericUpDown10" + t.ToString(), true)[0]).Value))[0];  //amount
                        //EntireBytes[SupplyInfoStart + p] = BitConverter.GetBytes(decimal.ToInt32(((NumericUpDown)this.Controls.Find("numericUpDown10" + t.ToString(), true)[0]).Value))[0];

                        string spstr = ((TextBox)this.Controls.Find("textBox10" + t.ToString(), true)[0]).Text;
                        string spid = List.Item.FirstOrDefault(x => x.Value == spstr).Key.ToString();
                        if (spid.Length == 1)
                        {
                            spid = "00000" + spid;
                        }
                        else if (spid.Length == 2)
                        {
                            spid = "0000" + spid;
                        }
                        else if (spid.Length == 3)
                        {
                            spid = "000" + spid;
                        }
                        else if (spid.Length == 4)
                        {
                            spid = "00" + spid;
                        }
                        else if (spid.Length == 6)
                        {
                            spid = "0" + spid;
                        }
                        byte[] spbyte = BitConverter.GetBytes(Convert.ToInt32(spid));
                        Temprewardarray[p - 2] = spbyte[0];
                        Temprewardarray[p - 1] = spbyte[1];
                    }

                }
                else if (i < 40)
                {
                    for (int t = 10; t < 40; t += 1)
                    {
                        int p = t * 4 + 2;
                        Temprewardarray[p] = BitConverter.GetBytes(decimal.ToInt32(((NumericUpDown)this.Controls.Find("numericUpDown1" + t.ToString(), true)[0]).Value))[0];

                        string spstr = ((TextBox)this.Controls.Find("textBox1" + t.ToString(), true)[0]).Text;
                        string spid = List.Item.FirstOrDefault(x => x.Value == spstr).Key.ToString();
                        if (spid.Length == 1)
                        {
                            spid = "00000" + spid;
                        }
                        else if (spid.Length == 2)
                        {
                            spid = "0000" + spid;
                        }
                        else if (spid.Length == 3)
                        {
                            spid = "000" + spid;
                        }
                        else if (spid.Length == 4)
                        {
                            spid = "00" + spid;
                        }
                        else if (spid.Length == 6)
                        {
                            spid = "0" + spid;
                        }
                        byte[] spbyte = BitConverter.GetBytes(Convert.ToInt32(spid));
                        Temprewardarray[p - 2] = spbyte[0];
                        Temprewardarray[p - 1] = spbyte[1];
                    }
                }
            }
            EntireBytes.AddRange(Temprewardarray);
            #endregion
            //Lrg mosnter
            #region Lrg mosnter
            int tempnum = EntireBytes.Count;
            byte[] tempnum1 = BitConverter.GetBytes(Convert.ToInt16(tempnum));
            EntireBytes[24] = tempnum1[0];  //replace new header
            EntireBytes[25] = tempnum1[1];

            byte[] tempLMinfo = { 01, 00, 00, 00, 00, 00, 00, 00, 00, 10, 00, 00, 20, 10, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 };

            tempnum = (EntireBytes.Count) + 32;
            tempnum1 = BitConverter.GetBytes(Convert.ToInt16(tempnum));
            tempLMinfo[8] = tempnum1[0];        //replace new header
            tempLMinfo[9] = tempnum1[1];

            tempnum = tempnum + 32;
            tempnum1 = BitConverter.GetBytes(Convert.ToInt16(tempnum));
            tempLMinfo[12] = tempnum1[0];       //replace new header
            tempLMinfo[13] = tempnum1[1];
            EntireBytes.AddRange(tempLMinfo);

            string templateMonsterHexData = textBox1.Text;
            var templateMonsterData = new List<byte>();
            for (int i = 0; i < templateMonsterHexData.Length / 2; i++)
            {
                templateMonsterData.Add(Convert.ToByte(templateMonsterHexData.Substring(i * 2, 2), 16));
            }

            byte[] tempLMinfotemo = { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 255, 255, 255, 255, 255, 255, 255, 255, 00, 00, 00, 00 };
            var tempLmdata = new List<byte>();

            int LMons1 = List.Monster1.FirstOrDefault(x => x.Value == comboBox_Lar1Boss_Monster.Text).Key;
            tempLMinfotemo[0] = BitConverter.GetBytes(LMons1)[0];
            byte[] templateMonsterData1 = new byte[60];
            templateMonsterData.CopyTo(templateMonsterData1, 0);
            templateMonsterData1[0] = BitConverter.GetBytes(LMons1)[0];

            string input1 = comboBox_Lar1Boss_SpawnArea.Text;
            var areaid1 = new List<byte>();
            for (int i = 0; i < input1.Length / 2; i++)
            {
                areaid1.Add(Convert.ToByte(input1.Substring(i * 2, 2), 16));
            }


            int LMons2 = List.Monster1.FirstOrDefault(x => x.Value == comboBox17.Text).Key;
            tempLMinfotemo[4] = BitConverter.GetBytes(LMons2)[0];
            byte[] templateMonsterData2 = new byte[60];
            templateMonsterData.CopyTo(templateMonsterData2, 0);
            templateMonsterData2[0] = BitConverter.GetBytes(LMons2)[0];

            string input2 = comboBox18.Text;
            var areaid2 = new List<byte>();
            for (int i = 0; i < input2.Length / 2; i++)
            {
                areaid2.Add(Convert.ToByte(input2.Substring(i * 2, 2), 16));
            }
            templateMonsterData2[8] = areaid2[0];


            int LMons3 = List.Monster1.FirstOrDefault(x => x.Value == comboBox19.Text).Key;
            tempLMinfotemo[8] = BitConverter.GetBytes(LMons3)[0];
            byte[] templateMonsterData3 = new byte[60];
            templateMonsterData.CopyTo(templateMonsterData3, 0);
            templateMonsterData3[0] = BitConverter.GetBytes(LMons3)[0];

            string input3 = comboBox20.Text;
            var areaid3 = new List<byte>();
            for (int i = 0; i < input3.Length / 2; i++)
            {
                areaid3.Add(Convert.ToByte(input3.Substring(i * 2, 2), 16));
            }
            templateMonsterData3[8] = areaid3[0];



            int LMons4 = List.Monster1.FirstOrDefault(x => x.Value == comboBox21.Text).Key;
            tempLMinfotemo[12] = BitConverter.GetBytes(LMons4)[0];
            byte[] templateMonsterData4 = new byte[60];
            templateMonsterData.CopyTo(templateMonsterData4, 0);
            templateMonsterData4[0] = BitConverter.GetBytes(LMons4)[0];

            string input4 = comboBox22.Text;
            var areaid4 = new List<byte>();
            for (int i = 0; i < input4.Length / 2; i++)
            {
                areaid4.Add(Convert.ToByte(input4.Substring(i * 2, 2), 16));
            }
            templateMonsterData4[8] = areaid4[0];


            EntireBytes.AddRange(tempLMinfotemo);
            EntireBytes.AddRange(templateMonsterData1);
            EntireBytes.AddRange(templateMonsterData2);
            EntireBytes.AddRange(templateMonsterData3);
            EntireBytes.AddRange(templateMonsterData4);
            EntireBytes.AddRange(Endofline);
            #endregion










            //Write new text offset
            //写入新的文本偏移指针
            eb2 = EntireBytes.ToArray();
            eb2[readPointer - 28] = mo[0];
            eb2[readPointer - 28 + 1] = mo[1];
            eb2[readPointer - 24] = sa[0];
            eb2[readPointer - 24 + 1] = sa[1];
            eb2[readPointer - 20] = sb[0];
            eb2[readPointer - 20 + 1] = sb[1];
            eb2[readPointer - 16] = cc[0];
            eb2[readPointer - 16 + 1] = cc[1];
            eb2[readPointer - 12] = fc[0];
            eb2[readPointer - 12 + 1] = fc[1];
            eb2[readPointer - 8] = em[0];
            eb2[readPointer - 8 + 1] = em[1];
            eb2[readPointer - 4] = tx[0];
            eb2[readPointer - 4 + 1] = tx[1];

            //Fee
            byte[] feeb = BitConverter.GetBytes(decimal.ToInt32(numericUpDown_fee.Value));
            eb2[204] = feeb[0];
            eb2[205] = feeb[1];
            eb2[206] = feeb[2];

            //Penalty
            byte[] penalty = BitConverter.GetBytes(decimal.ToInt32(numericUpDown_Penalty.Value));
            eb2[212] = penalty[0];
            eb2[213] = penalty[1];
            eb2[214] = penalty[2];

            //Main
            string mat = List.ObjectiveType.FirstOrDefault(x => x.Value == comboBox_MainType.Text).Key;
            byte[] madata = Enumerable.Range(0, mat.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(mat.Substring(x, 2), 16)).ToArray();
            eb2[240] = madata[0];
            eb2[241] = madata[1];
            eb2[242] = madata[2];
            eb2[243] = madata[3];

            if (madata[0] == 02 & madata[1] == 00)
            {
                int mot = List.Item.FirstOrDefault(x => x.Value == textBox_MainTarget.Text).Key;
                byte[] motitem = BitConverter.GetBytes(mot);
                eb2[244] = motitem[0];
                eb2[245] = motitem[1];
            }
            else
            {
                string mot = List.Monster.FirstOrDefault(x => x.Value == textBox_MainTarget.Text).Key;
                eb2[244] = Convert.ToByte(mot, 16);
            }

            decimal MainAmount = numericUpDown_MainAmount.Value;
            if (comboBox_MainType.SelectedIndex == 4)
            {
                MainAmount = MainAmount / 100;
            }
            byte[] mamt = BitConverter.GetBytes(decimal.ToInt32(MainAmount));
            eb2[246] = mamt[0];
            eb2[247] = mamt[1];

            byte[] mrwd = BitConverter.GetBytes(decimal.ToInt32(numericUpDown_MainRewardMoney.Value));
            eb2[208] = mrwd[0];
            eb2[209] = mrwd[1];
            eb2[210] = mrwd[2];

            //Sub A
            string subat = List.ObjectiveType.FirstOrDefault(x => x.Value == comboBox_SubAType.Text).Key;
            if (!(subat == "00000000"))
            {
                byte[] subadata = Enumerable.Range(0, subat.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(subat.Substring(x, 2), 16)).ToArray();
                eb2[248] = subadata[0];
                eb2[249] = subadata[1];
                eb2[250] = subadata[2];
                eb2[251] = subadata[3];

                if (subadata[0] == 02 & subadata[1] == 00)
                {
                    int subatt = List.Item.FirstOrDefault(x => x.Value == textBox_SubATarget.Text).Key;
                    byte[] subaitem = BitConverter.GetBytes(subatt);
                    eb2[252] = subaitem[0];
                    eb2[253] = subaitem[1];
                }
                else
                {
                    string subatt = List.Monster.FirstOrDefault(x => x.Value == textBox_SubATarget.Text).Key;
                    eb2[252] = Convert.ToByte(subatt, 16);
                    eb2[253] = 0;
                }

                decimal AAmount = numericUpDown_SubA_Amount.Value;
                if (comboBox_SubAType.SelectedIndex == 4)
                {
                    AAmount = AAmount / 100;
                }
                byte[] subaamt = BitConverter.GetBytes(decimal.ToInt32(AAmount));
                eb2[254] = subaamt[0];
                eb2[255] = subaamt[1];

                byte[] subatrd = BitConverter.GetBytes(decimal.ToInt32(numericUpDown_SubARewardMoney.Value));
                eb2[216] = subatrd[0];
                eb2[217] = subatrd[1];
                eb2[218] = subatrd[2];
            }

            //Sub B
            string subbt = List.ObjectiveType.FirstOrDefault(x => x.Value == comboBox_SubBType.Text).Key;
            if (!(subbt == "00000000"))
            {
                byte[] subbdata = Enumerable.Range(0, subbt.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(subbt.Substring(x, 2), 16)).ToArray();
                eb2[256] = subbdata[0];
                eb2[257] = subbdata[1];
                eb2[258] = subbdata[2];
                eb2[259] = subbdata[3];

                if (subbdata[0] == 02 & subbdata[1] == 00)
                {
                    int subbtt = List.Item.FirstOrDefault(x => x.Value == textBox_SubBTarget.Text).Key;
                    byte[] subbitem = BitConverter.GetBytes(subbtt);
                    eb2[260] = subbitem[0];
                    eb2[261] = subbitem[1];
                }
                else
                {
                    string subbtt = List.Monster.FirstOrDefault(x => x.Value == textBox_SubBTarget.Text).Key;
                    eb2[260] = Convert.ToByte(subbtt, 16);
                    eb2[261] = 0;
                }

                decimal BAmount = numericUpDown_SubB_Amount.Value;
                if (comboBox_SubBType.SelectedIndex == 4)
                {
                    BAmount = BAmount / 100;
                }
                byte[] subbamt = BitConverter.GetBytes(decimal.ToInt32(BAmount));
                eb2[262] = subbamt[0];
                eb2[263] = subbamt[1];

                byte[] subbtrd = BitConverter.GetBytes(decimal.ToInt32(numericUpDown_SubBRewardMoney.Value));
                eb2[220] = subbtrd[0];
                eb2[221] = subbtrd[1];
                eb2[222] = subbtrd[2];
            }

            //Monster
            #region 怪物设置
            int MainMonstPointer = BitConverter.ToInt32(ba, 24);
            string MonsID1 = List.Monster.FirstOrDefault(x => x.Value == comboBox_Lar1Boss_Monster.Text).Key;
            eb2[MainMonstPointer + 32] = Convert.ToByte(MonsID1, 16);
            eb2[MainMonstPointer + 64] = Convert.ToByte(MonsID1, 16);

            eb2[72] = BitConverter.GetBytes(decimal.ToInt16(numericUpDown_LarBoss_Strenght.Value))[0];      //str
            eb2[68] = BitConverter.GetBytes(decimal.ToInt16(numericUpDown_LarBoss_Size.Value))[0];     //size
            eb2[70] = BitConverter.GetBytes(decimal.ToInt16(numericUpDown1_LarBoss_SizeRange.Value))[0];     //size range
            switch (comboBox_LarBoss_Species.SelectedIndex)
            {
                case 0:
                    eb2[337] = 0;
                    break;
                case 1:
                    eb2[337] = 1;
                    break;
                case 2:
                    eb2[337] = 2;
                    break;
                case 3:
                    eb2[337] = 9;
                    break;
                case 4:
                    eb2[337] = 10;
                    break;
                case 5:
                    eb2[337] = 16;
                    break;
                case 6:
                    eb2[337] = 0;
                    break;
            }
            eb2[92] = Convert.ToByte(List.Rank.FirstOrDefault(x => x.Value == comboBox_LarBoss_Carve.Text).Key);     //carve rank

            eb2[97] = BitConverter.GetBytes(decimal.ToInt16(numericUpDown_SmallMonsterStrength.Value))[0];     //small


            byte[] HRP1 = BitConverter.GetBytes(decimal.ToInt32(numericUpDown_MainHRP.Value));
            eb2[76] = HRP1[0];
            eb2[77] = HRP1[1];
            eb2[78] = HRP1[2];
            byte[] HRP2 = BitConverter.GetBytes(decimal.ToInt32(numericUpDown_SubAHRP.Value));
            eb2[84] = HRP2[0];
            eb2[85] = HRP2[1];
            eb2[86] = HRP2[2];
            byte[] HRP3 = BitConverter.GetBytes(decimal.ToInt32(numericUpDown_SubBHRP.Value));
            eb2[88] = HRP3[0];
            eb2[89] = HRP3[1];
            eb2[90] = HRP3[2];
            #endregion

            //Clear condition
            #region Clear condition
            if (comboBox_ClearCondition.SelectedIndex == 0)
            {
                eb2[264] = 2;
            }
            else if (comboBox_ClearCondition.SelectedIndex == 1)
            {
                eb2[264] = 3;
            }
            else
            {
                eb2[264] = 4;
            }
            #endregion

            //Another target
            #region Another target
            if (!(numericUpDown_AnotherTgtNumber.Value == 0))
            {
                eb2[128] = BitConverter.GetBytes(comboBox_AnotherTargetType.SelectedIndex + 3)[0];
                eb2[130] = Convert.ToByte(MonsID1, 16);
                eb2[132] = BitConverter.GetBytes(decimal.ToInt16(numericUpDown_AnotherTgtNumber.Value))[0];
            }
            else
            {
                eb2[128] = 0;
                eb2[130] = 0;
                eb2[132] = 0;
            }

            byte[] QuestTime = BitConverter.GetBytes(decimal.ToInt32(numericUpDown_Time_Min.Value * 30 * 60));     //time
            eb2[224] = QuestTime[0];
            eb2[225] = QuestTime[1];
            eb2[226] = QuestTime[2];
            eb2[227] = QuestTime[3];

            byte[] NumStar = BitConverter.GetBytes(decimal.ToInt16(numericUpDown_StarLevel.Value));     //difficulty
            eb2[196] = NumStar[0];
            eb2[197] = NumStar[1];
            #endregion

            File.WriteAllBytes(path, eb2.ToArray());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(comboBox1.Text);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(comboBox2.Text);
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1006_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabPage6_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown92_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown68_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown20_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
