export interface User{
  username: string; //to make it optional use this username?: string;
  knownAs?: string;
  gender: string;
  token: string;
  photoUrl?: string;
}
