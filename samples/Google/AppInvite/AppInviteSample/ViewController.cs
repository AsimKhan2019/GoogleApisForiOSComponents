// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using UIKit;
using Google.SignIn;
using Google.AppInvite;

namespace AppInviteSample
{
	public partial class ViewController : UIViewController, IInviteDelegate, ISignInDelegate, ISignInUIDelegate
	{
		public ViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// TODO(developer) Configure the sign-in button look/feel
			SignIn.SharedInstance.Delegate = this;
			SignIn.SharedInstance.UIDelegate = this;

			// Sign in automatically.
			SignIn.SharedInstance.SignInUserSilently ();

			SetupUI ();
			ToggleAuthUI ();
		}

		void SetupUI ()
		{
			float grayValue = (204.0f/255);

			var grayColor = UIColor.FromRGBA (grayValue, grayValue, grayValue, 1.0f);

			inviteButton.Layer.CornerRadius = 3;
			inviteButton.Layer.ShadowRadius = 1;
			inviteButton.Layer.ShadowOffset = new CoreGraphics.CGSize (0, 0.5);
			inviteButton.Layer.ShadowColor = UIColor.Black.CGColor;
			inviteButton.Layer.ShadowOpacity = .7f;

			signOutButton.Layer.BorderWidth = .5f;
			signOutButton.Layer.BorderColor = grayColor.CGColor;
			signOutButton.Layer.CornerRadius = 2;
			signOutButton.Layer.ShadowRadius = .5f;
			signOutButton.Layer.ShadowOffset = new CoreGraphics.CGSize (0, 0.5);
			signOutButton.Layer.ShadowColor = UIColor.Black.CGColor;
			signOutButton.Layer.ShadowOpacity = .4f;

			disconnectButton.Layer.BorderWidth = .5f;
			disconnectButton.Layer.BorderColor = grayColor.CGColor;
			disconnectButton.Layer.CornerRadius = 2;
			disconnectButton.Layer.ShadowRadius = .5f;
			disconnectButton.Layer.ShadowOffset = new CoreGraphics.CGSize (0, 0.5);
			disconnectButton.Layer.ShadowColor = UIColor.Black.CGColor;
			disconnectButton.Layer.ShadowOpacity = .4f;
		}

		public void DidSignIn (SignIn signIn, GoogleUser user, NSError error)
		{
			// Perform any operations on signed in user here.
			statusText.Text = string.Format ("Signed in as {0}", user.Profile.Name);
			ToggleAuthUI ();
		}

		[Export ("signIn:didDisconnectWithUser:withError:")]
		public virtual void DidDisconnect (SignIn signIn, GoogleUser user, NSError error)
		{
			// Perform any operations when the user disconnects from app here.
			statusText.Text = "Disconnected user";
			ToggleAuthUI ();
		}

		partial void signOutTapped (NSObject sender)
		{
			SignIn.SharedInstance.SignOutUser ();
			statusText.Text = "Signed out";
			ToggleAuthUI ();
		}

		partial void disconnectTapped (NSObject sender)
		{
			SignIn.SharedInstance.DisconnectUser ();
		}

		partial void inviteTapped (NSObject sender)
		{
			var inviteDialog = Invite.InviteDialog;
			inviteDialog.SetInviteDelegate (this);

			// NOTE: You must have the App Store ID set in your developer console project
			// in order for invitations to successfully be sent.
			var message = string.Format ("Try this out!\n -{0}",
				SignIn.SharedInstance.CurrentUser.Profile.Name);

			// A message hint for the dialog. Note this manifests differently depending on the
			// received invation type. For example, in an email invite this appears as the subject.
			inviteDialog.SetMessage (message);

			// Title for the dialog, this is what the user sees before sending the invites.
			inviteDialog.SetTitle ("App Invites Example");
			inviteDialog.SetDeepLink ("app_url");
			inviteDialog.Open ();
		}

		[Export ("inviteFinishedWithInvitations:error:")]
		public virtual void InviteFinished (string[] invitationIds, NSError error)
		{
			var message = error != null ? error.LocalizedDescription :
				string.Format ("{0} invites sent", invitationIds.Length);
				
			new UIAlertView ("Done", message, null, "OK").Show ();
		}

		void ToggleAuthUI ()
		{
			if (SignIn.SharedInstance.CurrentUser == null ||
				SignIn.SharedInstance.CurrentUser.Authentication == null) {
				// Not signed in
				if (signInButton != null)
					signInButton.Enabled = true;
				signOutButton.Enabled = false;
				disconnectButton.Enabled = false;
				inviteButton.Enabled = false;
				PerformSegue ("SignedOutScreen", this);
			} else {
				// Signed in
				if (signInButton != null)
					signInButton.Enabled = false;
				signOutButton.Enabled = true;
				disconnectButton.Enabled = true;
				inviteButton.Enabled = true;
			}
		}

		override public UIStatusBarStyle PreferredStatusBarStyle ()
		{
			return UIStatusBarStyle.LightContent;
		}

	}
}