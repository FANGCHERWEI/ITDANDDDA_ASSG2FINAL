using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using Firebase.Database;
using System;
using TMPro;

public class Authentication : MonoBehaviour
{
    // initialise firebase auth
    Firebase.Auth.FirebaseAuth auth;
    public DatabaseReference dbReference;

    public GameObject loginPage;
    public GameObject gamePage;

    public static bool firstTimeLogin = false;
    public static bool loggedIn = false;

    public static string userID;
    public static string email;

    public GameObject usernameInputSignUp;
    public GameObject emailInputSignUp;
    public GameObject passwordInputSignUp;

    public static string usernameSignUp;
    public string emailSignUp;
    public string passwordSignUp;

    public string errorSignUpMessage;
    public GameObject errorSignUpUI;

    public GameObject emailInputLogin;
    public GameObject passwordInputLogin;

    public string emailLogin;
    public string passwordLogin;

    public string errorLoginMessage;
    public GameObject errorLoginUI;

    public GameObject forgetPasswordInput;
    public string forgetPasswordEmail;

    public string errorForgetPassword;
    public GameObject errorForgetPasswordUI;


    // initialise auth instance
    private void Awake()
    {
        auth = FirebaseAuth.DefaultInstance;
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // updates the error messages of sign up and login
        errorSignUpUI.GetComponent<TMP_Text>().text = errorSignUpMessage;
        errorLoginUI.GetComponent<TMP_Text>().text = errorLoginMessage;

        // if login is successful, bring user to home menu
        if (loggedIn)
        {
            loginPage.SetActive(false);
            gamePage.SetActive(true);
        }
    }

    // signing up the user
    public void SigningUp()
    {
        // get the player input for the username during sign up and store it in usernameSignUp;
        usernameSignUp = usernameInputSignUp.GetComponent<TMP_InputField>().text;

        // checks if the username field is empty, adding a few variants of empty field
        if (usernameSignUp == "" || usernameSignUp == " " || usernameSignUp == "   ")
        {
            errorSignUpMessage = "Please enter a username";
            return;
        }

        // get the player input for the email during sign up and store it in emailSignUp
        emailSignUp = emailInputSignUp.GetComponent<TMP_InputField>().text;
        // get the player input for the password during sign up and store it in passwordSignUp
        passwordSignUp = passwordInputSignUp.GetComponent<TMP_InputField>().text;

        // start to create user with email and password
        auth.CreateUserWithEmailAndPasswordAsync(emailSignUp, passwordSignUp).ContinueWith(task =>
        {
            // perform task handling
            // if task fails or gets canceled
            if (task.IsFaulted || task.IsCanceled)
            {
                // show error
                Debug.LogError("Sorry, there was an error creating your account, ERROR: " + task.Exception);
                // update error sign up message
                errorSignUpMessage = "Please enter a valid email and password";
                return;
            }

            // if task is completed
            else if (task.IsCompleted)
            {
                // get the result
                Firebase.Auth.FirebaseUser newPlayer = task.Result;
                Debug.LogFormat("Welcome to my new game, {0}", newPlayer.Email);
                // store User ID into the userID variable
                userID = newPlayer.UserId;
                // store email into the email variable
                email = newPlayer.Email;
                // update first time login variable to be true
                firstTimeLogin = true;
                // update logged in variable to be true
                loggedIn = true;
                // create new player in database
                CreateNewPlayer(newPlayer.UserId, usernameSignUp, newPlayer.Email, true);
                // create high scores for the player in the database
                Leaderboard(newPlayer.UserId, 0, 0, 0, 0, "Null", usernameSignUp);
                // create allow levels in the database
                AllowLevels(newPlayer.UserId, false, false, false);
                return;
            }
        });

    }

    // creates a new player in firebase with their userID, username, email, and active status
    public void CreateNewPlayer(string uuid, string userName, string email, bool active)
    {
        // references to the class
        NewPlayer createNewPlayer = new NewPlayer(userName, email, active);

        // set the path to where the data would be stored
        dbReference.Child("players/" + uuid).SetRawJsonValueAsync(createNewPlayer.NewPlayerToJson());

    }

    // creates the neccesary variables to store players' high scores for each level and their average high scores
    public void Leaderboard(string uuid, float level1HighScore, float level2HighScore, float averageHighScore, float totalTimePlayed, string userProfileRating, string userName)
    {
        // references to the class
        Leaderboard defaultScore = new Leaderboard(level1HighScore, level2HighScore, averageHighScore, totalTimePlayed, userProfileRating, userName);

        // set the path to where the data would be stored
        dbReference.Child("leaderboards/" + uuid).SetRawJsonValueAsync(defaultScore.LeaderboardToJson());
    }

    // creates the allow levels to check if player has finish the previous levels before continuing
    public void AllowLevels(string uuid, bool level1Played, bool level2Played, bool allowLevel2)
    {
        // references to the class
        AllowLevels levels = new AllowLevels(level1Played, level2Played, allowLevel2);

        // set the path to where the data would be stored
        dbReference.Child("allowLevels/" + uuid).SetRawJsonValueAsync(levels.AllowLevelsToJson());
    }

    // logging the user in
    public void LoggingIn()
    {
        // get the player input for the email during login and store it in emailLogin
        emailLogin = emailInputLogin.GetComponent<TMP_InputField>().text;
        // get the player input for the password during login and store it in passwordLogin
        passwordLogin = passwordInputLogin.GetComponent<TMP_InputField>().text;

        // start the sign in with email and password
        auth.SignInWithEmailAndPasswordAsync(emailLogin, passwordLogin).ContinueWith(task =>
        {
            // if task fails or canceled
            if (task.IsFaulted || task.IsCanceled)
            {
                // show error
                Debug.LogError("Sorry, there was an error logging into your account, ERROR: " + task.Exception);
                // update the error login message
                errorLoginMessage = "Wrong email or password. Please try again.";
                return;
            }

            // if task is completed
            else if (task.IsCompleted)
            {
                // get the result
                Firebase.Auth.FirebaseUser User = task.Result;
                Debug.LogFormat("Welcome to my new game, {0}", User.Email);
                // store the User ID into the userID variable
                userID = User.UserId;
                // store the email into the email variable
                email = User.Email;
                // update logged in variable to be true
                loggedIn = true;
                // update last logged in time in the database
                UpdateLoginTime();
                return;
            }
        });
    }

    // user forgets password
    public void ForgetPassword()
    {
        // get the player email that they want to reset the password for
        forgetPasswordEmail = forgetPasswordInput.GetComponent<TMP_InputField>().text;

        // send password reset email
        auth.SendPasswordResetEmailAsync(forgetPasswordEmail).ContinueWith(task =>
        {
            // if task is canceled
            if (task.IsCanceled)
            {
                // send an error
                Debug.LogError("SendPasswordResetEmailAsync was canceled");
                return;
            }

            // if task is faulted
            else if (task.IsFaulted)
            {
                // send an error
                Debug.LogError("SendPasswordResetAsync encountered an error " + task.Exception);
                // updates the error forget message
                errorForgetPassword = "Please enter a valid email.";
                return;
            }

            else if (task.IsCompleted)
            {
                errorForgetPassword = "Password reset email is sent successfully.";
            }
        });
    }

    // update the last logged in timestamp for the player
    public void UpdateLoginTime()
    {
        // timestamp properties
        var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        // updates the last logged in timestamp for the player
        dbReference.Child("players/" + userID + "/lastLoggedIn").SetValueAsync(timestamp);
    }

    // signing out the player
    public void SigningOut()
    {
        if (auth.CurrentUser != null)
        {
            // sign the user out
            auth.SignOut();

            // if it is the user's first time login, log out
            if (firstTimeLogin)
            {
                firstTimeLogin = false;
            }

            // update logged in variable to be false
            loggedIn = false;
            
            // reset the userID and username variable from another script
            SaveLevelData.userIDRetrieved = false;
            SaveLevelData.getUsernameFlag = true;
            SaveLevelData.highScoresRetrived = false;

            Debug.Log("User has been logged out");
        }
    }
}
