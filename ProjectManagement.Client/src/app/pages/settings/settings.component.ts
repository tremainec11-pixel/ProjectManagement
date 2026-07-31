import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
selector: 'app-settings',
standalone: true,
imports: [
CommonModule,
FormsModule
],
templateUrl: './settings.component.html',
styleUrl: './settings.component.css'
})
export class SettingsComponent implements OnInit {

// =========================
// ACTIVE SETTINGS SECTION
// =========================

activeSection = 'profile';

// =========================
// APPEARANCE
// =========================

selectedTheme: 'light' | 'dark' = 'light';

// =========================
// PASSWORD FORM VISIBILITY
// =========================

showPasswordForm = false;

// =========================
// SAVE CONFIRMATIONS
// =========================

notificationSaved = false;

profileSaved = false;

// =========================
// PROFILE
// =========================

profile = {
firstName: 'Tremaine',
lastName: 'Charles',
email: '[tremaine@example.com](mailto:tremaine@example.com)'
};

// =========================
// NOTIFICATIONS
// =========================

notifications = {
emailNotifications: true,
projectUpdates: true,
taskAssignments: true,
weeklySummary: false
};

// =========================
// PASSWORD
// =========================

passwordData = {
currentPassword: '',
newPassword: '',
confirmPassword: ''
};

// =========================
// INITIALIZE
// =========================

ngOnInit(): void {


this.loadTheme();


}

// =========================
// LOAD SAVED THEME
// =========================

loadTheme(): void {


const savedTheme =
  localStorage.getItem('app-theme');

if (
  savedTheme === 'dark' ||
  savedTheme === 'light'
) {

  this.selectedTheme = savedTheme;

}

this.applyTheme(this.selectedTheme);


}

// =========================
// CHANGE SETTINGS SECTION
// =========================

changeSection(section: string): void {


this.activeSection = section;

// Close password form
this.showPasswordForm = false;


}

// =========================
// SAVE PROFILE
// =========================

saveProfile(): void {


console.log(
  'Profile saved:',
  this.profile
);

this.profileSaved = true;

setTimeout(() => {

  this.profileSaved = false;

}, 3000);


}

// =========================
// SAVE NOTIFICATIONS
// =========================

saveNotifications(): void {


console.log(
  'Notifications saved:',
  this.notifications
);

this.notificationSaved = true;

setTimeout(() => {

  this.notificationSaved = false;

}, 3000);


}

// =========================
// CHANGE PASSWORD
// =========================

changePassword(): void {

this.showPasswordForm = true;


}

// =========================
// CANCEL PASSWORD CHANGE
// =========================

cancelPasswordChange(): void {


this.showPasswordForm = false;

this.passwordData = {
  currentPassword: '',
  newPassword: '',
  confirmPassword: ''
};


}

// =========================
// SAVE PASSWORD
// =========================

savePassword(): void {


// Validate empty fields

if (
  !this.passwordData.currentPassword ||
  !this.passwordData.newPassword ||
  !this.passwordData.confirmPassword
) {

  alert(
    'Please complete all password fields.'
  );

  return;

}


// Validate password confirmation

if (
  this.passwordData.newPassword !==
  this.passwordData.confirmPassword
) {

  alert(
    'New password and confirmation do not match.'
  );

  return;

}


// Password updated

console.log(
  'Password updated successfully'
);

alert(
  'Password updated successfully.'
);


// Reset password form

this.cancelPasswordChange();


}

// =========================
// CHANGE THEME
// =========================

changeTheme(
theme: 'light' | 'dark'
): void {


this.selectedTheme = theme;

this.applyTheme(theme);

// Save theme preference

localStorage.setItem(
  'app-theme',
  theme
);

console.log(
  'Theme changed to:',
  theme
);



}

// =========================
// APPLY GLOBAL THEME
// =========================

private applyTheme(
theme: 'light' | 'dark'
): void {


const body =
  document.body;

// Remove previous theme

body.classList.remove(
  'light-theme',
  'dark-theme'
);

// Add selected theme

body.classList.add(
  `${theme}-theme`
);


}

}
