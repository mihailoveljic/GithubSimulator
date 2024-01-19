export class UpdateUserDto {
    name: string = "";
    surname: string = "";
    email: string = "";
    username: string = "";
    password: string = "";

    constructor(name: string, surname: string, email: string, username: string, password: string) {
        this.name = name;
        this.surname = surname;
        this.email = email;
        this.username = username;
        this.password = password;
    }
}
