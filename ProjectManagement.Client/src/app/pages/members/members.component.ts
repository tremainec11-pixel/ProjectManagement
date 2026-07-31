
import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { UserService, User } from '../../core/services/user.service';

@Component({
  selector: 'app-members',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './members.component.html',
  styleUrl: './members.component.css'
})
export class MembersComponent implements OnInit {

  private readonly userService = inject(UserService);

  users: User[] = [];
  filteredUsers: User[] = [];

  searchTerm = '';

  isLoading = false;
  errorMessage = '';

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {

    this.isLoading = true;
    this.errorMessage = '';

    this.userService.getUsers().subscribe({

      next: (users) => {

        console.log(
          'USERS LOADED FOR MEMBERS:',
          users
        );

        this.users = users;
        this.filteredUsers = users;

        this.isLoading = false;
      },

      error: (error) => {

        console.error(
          'ERROR LOADING MEMBERS:',
          error
        );

        this.errorMessage =
          'Unable to load members. Please try again.';

        this.isLoading = false;
      }

    });

  }

  searchMembers(): void {

    const search =
      this.searchTerm
        .trim()
        .toLowerCase();

    // Si el buscador está vacío,
    // mostramos todos los usuarios.

    if (!search) {

      this.filteredUsers = this.users;

      return;
    }

    // Filtrar por nombre, apellido o email.

    this.filteredUsers =
      this.users.filter(user => {

        const firstName =
          user.firstName?.toLowerCase() || '';

        const lastName =
          user.lastName?.toLowerCase() || '';

        const email =
          user.email?.toLowerCase() || '';

        const fullName =
          `${firstName} ${lastName}`;

        return (
          firstName.includes(search) ||
          lastName.includes(search) ||
          fullName.includes(search) ||
          email.includes(search)
        );

      });

  }

}

