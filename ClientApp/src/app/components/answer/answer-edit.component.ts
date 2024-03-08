import { HttpClient } from "@angular/common/http";
import { Component, Inject, Input, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { Answer } from "../../interfaces/answer";

@Component({
  selector: "answer-edit",
  templateUrl: './answer-edit.component.html',
  styleUrls: ['./answer-edit.component.css']
})

export class AnswerEditComponent {
  title: string | undefined;
  answer: Answer;

  // Takes True in case of editing existing answer,
  // and False in other case.
  editMode: boolean;

  constructor(private activatedRoute: ActivatedRoute,
    private router: Router,
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string) {
    this.baseUrl = "https://localhost:7136/";

    // Create empty object based on interface answer
    this.answer = <Answer>{};

    var id = +this.activatedRoute.snapshot.params["id"];

    // Verify, if it's edit mode
    this.editMode = (this.activatedRoute.snapshot.url[1].path === "edit");

    if (this.editMode) {

      // Download answer from the server
      var url = this.baseUrl + "api/answer/" + id;
      this.http.get<Answer>(url).subscribe(result => {
        this.answer = result;
        this.title = "Edition - " + this.answer.Text;
      }, error => console.error(error));
    }
    else {
      this.answer.QuestionId = id;
      this.title = "Create new answer";
    }
  }

  onSubmit(answer: Answer) {
    var url = this.baseUrl + "api/answer/";

    if (this.editMode) {
      this.http
        .post<Answer>(url, answer)
        .subscribe(res => {
          var v = res;
          console.log("Answer " + v.Id + " was updated.");
          this.router.navigate(["question/edit", v.QuestionId]);
        }, error => console.log(error));
    }
    else {
      this.http
        .put<Answer>(url, answer)
        .subscribe(res => {
          var v = res;
          console.log("Answer " + v.Id + " was created.");
          this.router.navigate(["question/edit", v.QuestionId]);
        }, error => console.log(error));
    }
  }

  onBack() {
    this.router.navigate(["question/edit", this.answer.QuestionId]);
  }
}


