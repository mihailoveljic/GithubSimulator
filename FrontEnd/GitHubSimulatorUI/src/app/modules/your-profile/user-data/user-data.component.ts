import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UpdateUserDto } from 'src/app/dto/updateUserDto';
import { UserDto } from 'src/app/dto/userDto';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-user-data',
  templateUrl: './user-data.component.html',
  styleUrls: ['./user-data.component.scss']
})
export class UserDataComponent implements OnInit {

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