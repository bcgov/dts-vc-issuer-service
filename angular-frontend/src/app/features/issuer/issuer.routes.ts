export class IssuerRoutes {
  public static MODULE_PATH = 'issuer';

  public static LOGIN_PAGE = 'login';

  public static CREDENTIALS_PAGE = 'credentials';
  public static ISSUANCE_PAGE = 'issuance';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${ route }`;
  }
}
