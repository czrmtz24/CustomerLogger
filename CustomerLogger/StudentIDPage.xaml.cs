﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CustomerLogger {
    /// <summary>
    /// Interaction logic for SudentIDPage.xaml
    /// </summary>
    
    //inital page the customer sees
    //gets their student ID number
    //will auto place a 0 in front if it is not automatically there
    //also we can have the card swiper attached to the comptuer so they
    //can swipe the cougarcard and it will work as if they typed it in
    //pretty nifty, because the card swiper just puts the number on the card
    //as if it can from stdin
    
    //because it is set to use the enter key to move on as well as clicking the next button,
    //if they swipe the card it should automatically save their number and move onto the next page
    //with out the user needing to click.
    public partial class StudentIDPage:Page {

        MainWindow _main_window;

        private string _student_id;
    
        public StudentIDPage(MainWindow mw) {
            InitializeComponent();
            _main_window = mw;
            //this.Width = _main_window.Width;
            //this.Height = _main_window.Height;

            SubmitButton.IsEnabled = false;
            StudentNumberTextBox.Focus();
        }

        public string StudentID {
            get { return _student_id; }
        }

        //when clicked we move on to the next page
        private void SubmitButton_Click(object sender, RoutedEventArgs e) {

            if(SubmitButton.IsEnabled) {
                _student_id = StudentNumberTextBox.Text;
                _main_window.changePage(_main_window.DevicePage);
            }

        }

        //grabs all text in the text box
        //incase customer thinks they did it wrong and then
        //wants to go back, it will highlight all of the text
        //woot woot for usability features
        private void StudentNumberTextBox_GotFocus(object sender, RoutedEventArgs e) {
            StudentNumberTextBox.SelectAll();
        }

        //when they type in the box this event will fire
        private void StudentNumberTextBox_TextChanged(object sender, TextChangedEventArgs e) {

            bool correct_length;
            bool is_num;

            //make sure we have a valid lenght, either 8 without a leading 0 or nine with a leading 0
            if(StudentNumberTextBox.Text.Length == 8 || (StudentNumberTextBox.Text.Length == 9 && StudentNumberTextBox.Text[0] == '0')) { // length of 9 is only correct if first digit is 0

                correct_length = true;
            } else {

                correct_length = false;
            }

            //make sure we actually have an integer as the student id number
            //no decimals
            //woot woot for input sanitization
            int n;
            is_num = int.TryParse(StudentNumberTextBox.Text, out n);

            //if we have a valid id number, IE correct length and its an int
            //then we can allow the customer to go on
            if(is_num && correct_length) {
                SubmitButton.IsEnabled = true;
            } else {
                SubmitButton.IsEnabled = false;
            }
        }

        //allows for enter key to be used as a click for the submit button
        private void StudentNumberTextBox_KeyUp(object sender, KeyEventArgs e) {
            if(e.Key == Key.Enter) {
                SubmitButton_Click(sender, e);
            }
        }
    }
}
