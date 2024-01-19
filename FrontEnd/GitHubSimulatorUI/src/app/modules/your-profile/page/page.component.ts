import { Component, OnInit } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field'; 
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UpdateUserDto } from 'src/app/dto/updateUserDto';
import { UserDto } from 'src/app/dto/userDto';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-page',
  templateUrl: './page.component.html',
  styleUrls: ['./page.component.scss']
})

export class PageComponent implements OnInit {

  username: string = '';
  name: string = '';
  surname: string = '';
  email: string = '';
  currentPassword: string = '';
  newPassword: string = '';
  confirmNewPassword: string = '';

  constructor(private userService: UserService, private router: Router, private toastr: ToastrService) { }

  ngOnInit() {
    this.getUserData();
  }

  passwordMismatch: boolean = false;

  checkPasswordMatch() {
    this.passwordMismatch = this.newPassword !== this.confirmNewPassword;
  }

  getUserData() {
    this.userService.getUser().subscribe(
      response => {
        this.username = response.username;
        this.name = response.name;
        this.surname = response.surname;
        this.email = response.email;
      },
      error => {
        if (error.status === 401 || error.status === 400) {
          console.log(error);
          this.toastr.error('Cannot get user data');
        } 
      }
    )
  }


  updateUserData() {
    const userDto = new UpdateUserDto(this.name, this.surname, this.email, this.username, "12345");

    this.userService.updateUser(userDto).subscribe(
      response => {
        this.username = response.username;
        this.name = response.name;
        this.surname = response.surname;
        this.email = response.email;
        this.toastr.success('You updated your data successfully!');
      },
      error => {
        if (error.status === 401 || error.status === 400) {
          console.log(error);
          this.toastr.error('Cannot update data');
        } 
      }
    )
  }

  updatePassword() {
    const userDto = new UpdateUserDto(this.name, this.surname, this.email, this.username, "12345");

    this.userService.updateUser(userDto).subscribe(
      response => {
        this.username = response.username;
        this.name = response.name;
        this.surname = response.surname;
        this.email = response.email;
        this.toastr.success('You updated your data successfully!');
      },
      error => {
        if (error.status === 401 || error.status === 400) {
          console.log(error);
          this.toastr.error('Cannot update data');
        } 
      }
    )
  }

}