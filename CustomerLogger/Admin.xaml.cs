﻿using System;
using System.Windows;
using System.Windows.Forms;

namespace CustomerLogger
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>


    //allows for admin controls of the program.
    //This includes starting/stopping logging, changing directories were things are saved and other usefull things
    public partial class AdminWindow:Window {

        MainWindow _main_window;
        System.Windows.Forms.FolderBrowserDialog _folder_dialog;

        public AdminWindow(MainWindow main_window) {
            InitializeComponent();
            _main_window = main_window;
            //displaying the current LogPath
            DirectoryTextBlock.Text = _main_window.LogPath;
            //display if we are currently logging
            if(_main_window.Logging) { 
            
                LogGoingTextBlock.Text = "Logging: Yes";
            } else {
                LogGoingTextBlock.Text = "Logging: No";
            }

            //display if emails are being sent when a customer logs in
            //this is how we get the tickets into ORTS
            if (_main_window.EmailLogging) {
                emailSendTextBlock.Text = "Emails Sending: Yes";
                emailButton.Content = "Disable Emails";
            }
            else {
                emailSendTextBlock.Text = "Emails Sending: No";
            }

            _folder_dialog = new System.Windows.Forms.FolderBrowserDialog();
            this.Activate();
        }

        // starting the day includes getting a new csv file setup for that day and then starting logging
        // it will also prompt you if you want to enable emails for OTRS
        private void StartDayButton_Click(object sender, RoutedEventArgs e) {
            
            if (_main_window.Logging) {
                System.Windows.MessageBox.Show("The day has already been started!", "Error"); // avoid being able to start a day if it is already started
                return;
            }

            _main_window.StartLog(DateTime.Now.ToString("MM-dd-yyyy")); //! this defaults to setting Logging to true and EmailLogging to true

            // ask if want to enable emails
            var result = System.Windows.Forms.MessageBox.Show("Would you like to enable emails?", "Email", MessageBoxButtons.YesNo);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                _main_window.EmailLogging = false; // set to false before calling next function so it can execute correctly and enable EmailLogging
            }
            emailButton_Click(new object(), new RoutedEventArgs()); //!! this will set main windows EmailLogging accordingly

            MessageWindow mw = new MessageWindow("Start New day \n" + DateTime.Now.ToString("MM-dd-yyyy"), 3.0);
            mw.Show();
            LogGoingTextBlock.Text = "Logging: Yes";

        }

        //end the day involves ending the log (writting the totals and then closing the file) and then turning off logging
        private void EndDayButton_Click(object sender, RoutedEventArgs e) {
            
            if (false == _main_window.Logging) {
                System.Windows.MessageBox.Show("The day has not been started!", "Error"); // avoid being able to end a day if it is not started
                return;
            }
            
            _main_window.EndLog();
            MessageWindow mw = new MessageWindow("End day \n" + DateTime.Now.ToString("MM-dd-yyyy"), 3.0);
            mw.Show();
            LogGoingTextBlock.Text = "Logging: No";
        }

        //changes the directory logs get saved to via the folder dialog
        private void LogDirectoryButton_Click(object sender, RoutedEventArgs e) {
            System.Windows.Forms.DialogResult result = _folder_dialog.ShowDialog();
            if(System.Windows.Forms.DialogResult.OK == result) {
                _main_window.LogPath = _folder_dialog.SelectedPath.ToString();
            }
            DirectoryTextBlock.Text = _main_window.LogPath;
        }

        //stops the program. Like all of it. Like it will shut down the program
        //don't click this unless you want the program to not be running any more.
        //I mean it
        //it will not be running after you click this button
        //you will need to restart it
        private void StopButton_Click(object sender, RoutedEventArgs e) {
            _main_window.Close();
            this.Close();
        }

        //closes the admin window - not the program. That is the job of 
        //the Stop Button
        private void CloseButton_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        //handles the emailing part of admin
        private void emailButton_Click(object sender, RoutedEventArgs e)
        {
            if (_main_window.EmailLogging == false)
            {
                PasswordWindow pw = new PasswordWindow();
                pw.ShowDialog();

                _main_window.EmailPassword = pw.Password;
                _main_window.EmailLogging = true;
                emailSendTextBlock.Text = "Emails Sending: Yes";
                emailButton.Content = "Disable Emails";
                System.Windows.MessageBox.Show("Email password set!");
            }
            else
            {
                _main_window.EmailPassword = ""; // clear out password when not sending email
                _main_window.EmailLogging = false;
                emailSendTextBlock.Text = "Emails Sending: No";
                emailButton.Content = "Email Login";
            }
        }
    }
}
