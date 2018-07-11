import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { baseUrl } from "../core/constants";
import { Company } from "./company.model";

@Injectable()
export class CompanyService {
  constructor(
    @Inject(baseUrl) private _baseUrl:string,
    private _client: HttpClient
  ) { }

  public get(): Observable<Array<Company>> {
    return this._client.get<{ companies: Array<Company> }>(`${this._baseUrl}api/companies`)
      .pipe(
        map(x => x.companies)
      );
  }

  public getById(options: { companyId: number }): Observable<Company> {
    return this._client.get<{ company: Company }>(`${this._baseUrl}api/companies/${options.companyId}`)
      .pipe(
        map(x => x.company)
      );
  }

  public remove(options: { company: Company }): Observable<void> {
    return this._client.delete<void>(`${this._baseUrl}api/companies/${options.company.companyId}`);
  }

  public save(options: { company: Company }): Observable<{ companyId: number }> {
    return this._client.post<{ companyId: number }>(`${this._baseUrl}api/companies`, { company: options.company });
  }
}
