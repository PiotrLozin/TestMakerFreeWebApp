import { HttpClient } from "@angular/common/http";
import { Component, Inject, Input, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { Quiz } from "../../interfaces/quiz";

@Component({
  selector: "result-edit",
  templateUrl: './result-edit.component.html',
  styleUrls: ['./result-edit.component.css']
})

export class ResultEditComponent {
  title: string | undefined;
  result: Result;

  // Takes True in case of editing existing result,
  // and False in other case.
  editMode: boolean;

  constructor(private activatedRoute: ActivatedRoute,
    private router: Router,
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string) {
    this.baseUrl = "https://localhost:7136/";

    // Create empty object based on interface result
    this.result = <Result>{};

    var id = +this.activatedRoute.snapshot.params["id"];

    // Verify, if it's edit mode
    this.editMode = (this.activatedRoute.snapshot.url[1].path === "edit");

    if (this.editMode) {

      // Download result from the server
      var url = this.baseUrl + "api/result/All/" + id;
      this.http.get<Result>(url).subscribe(result => {
        this.result = result;
        this.title = "Edition - " + this.result.Text;
      }, error => console.error(error));
    }
    else {
      this.result.QuizId = id;
      this.title = "Create new result";
    }
  }

  onSubmit(result: Result) {
    var url = this.baseUrl + "api/result/";

    if (this.editMode) {
      this.http
        .post<Result>(url, result)
        .subscribe(res => {
          var v = res;
          console.log("Result " + v.Id + " was updated.");
          this.router.navigate(["quiz/edit", v.QuizId]);
        }, error => console.log(error));
    }
    else {
      this.http
        .put<Result>(url, result)
        .subscribe(res => {
          var v = res;
          console.log("Result " + v.Id + " was created.");
          this.router.navigate(["quiz/edit", v.QuizId]);
        }, error => console.log(error));
    }
  }

  onBack() {
    this.router.navigate(["quiz/edit", this.result.QuizId]);
  }
}


