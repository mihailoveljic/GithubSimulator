export class UserDto {
    name: string = "";
    surname: string = "";
    email: string = "";
    username: string = "";

    constructor(name: string, surname: string, email: string, username: string) {
        this.name = name;
        this.surname = surname;
        this.email = email;
        this.username = username;
    }
}
