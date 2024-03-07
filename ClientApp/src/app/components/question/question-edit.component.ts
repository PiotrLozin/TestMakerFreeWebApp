import { HttpClient } from "@angular/common/http";
import { Component, Inject, Input, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { Quiz } from "../../interfaces/quiz";

@Component({
  selector: "question-edit",
  templateUrl: './question-edit.component.html',
  styleUrls: ['./question-edit.component.css']
})

export class QuestionEditComponent {
  title: string | undefined;
  question: Question;

  // Takes True in case of editing existing question,
  // and False in other case.
  editMode: boolean;

  constructor(private activatedRoute: ActivatedRoute,
    private router: Router,
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string) {
    this.baseUrl = "https://localhost:7136/";

    // Create empty object based on interface Question
    this.question = <Question>{};

    var id = +this.activatedRoute.snapshot.params["id"];

    // Verify, if it's edit mode
    this.editMode = (this.activatedRoute.snapshot.url[1].path === "edit");

    if (this.editMode) {

      // Download question from the server
      var url = this.baseUrl + "api/question/All/" + id;
      this.http.get<Question>(url).subscribe(result => {
        this.question = result;
        this.title = "Edition - " + this.question.Text;
      }, error => console.error(error));
    }
    else {
      this.question.QuizId = id;
      this.title = "Create new question";
    }
  }

  onSubmit(question: Question) {
    var url = this.baseUrl + "api/question/";

    if (this.editMode) {
      this.http
        .post<Question>(url, question)
        .subscribe(res => {
          var v = res;
          console.log("Question " + v.Id + " was updated.");
          this.router.navigate(["quiz/edit", v.QuizId]);
        }, error => console.log(error));
    }
    else {
      this.http
        .put<Question>(url, question)
        .subscribe(res => {
          var v = res;
          console.log("Question " + v.Id + " was created.");
          this.router.navigate(["quiz/edit", v.QuizId]);
        }, error => console.log(error));
    }
  }

  onBack() {
    this.router.navigate(["quiz/edit", this.question.QuizId]);
  }
}


