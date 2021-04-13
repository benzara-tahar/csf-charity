export interface State {
  isLoading: boolean;
  isLoggedIn: boolean;
  hasError: boolean;
  currentLanguage: string;
  error?:
    | {
        type: string;
        message: string;
      }
    | undefined;
}

export const initialState: State = {
  isLoading: false,
  isLoggedIn: false,
  hasError: false,
  currentLanguage: '',
  error: undefined,
};
