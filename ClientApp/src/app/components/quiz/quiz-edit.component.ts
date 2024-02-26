import { Component, Inject, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { HttpClient } from "@angular/common/http";
import { Quiz } from "../../interfaces/quiz";

@Component({
  selector: "quiz-edit",
  templateUrl: './quiz-edit.component.html',
  styleUrls: ['./quiz-edit.component.css']
})

export class QuizEditComponent {
  title: string | undefined;
  quiz: Quiz;

  // Will receive the value true if you edit an existing quiz
  // or false in case of new quiz
  editMode: boolean;

  constructor(private activatedRoute: ActivatedRoute,
    private router: Router,
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string)
  {
    this.baseUrl = "https://localhost:7136/";
   // Create empty object interface-compatible quiz
    this.quiz = <Quiz>{};

    var id = +this.activatedRoute.snapshot.params["id"];
    if (id) {
      this.editMode = true;
      // Upload quiz from server
      var url = this.baseUrl + "api/quiz/" + id;
      this.http.get<Quiz>(url).subscribe(result => {
        this.quiz = result;
        this.title = "Edition - " + this.quiz.Title;
      }, error => console.error(error));
    }
    else {
      this.editMode = false;
      this.title = "Create new quiz";
    }
  }

  onSubmit(quiz: Quiz) {
    var url = this.baseUrl + "api/quiz";

    if (this.editMode) {
      this.http.
        post<Quiz>(url, quiz)
        .subscribe(result => {
          var v = result;
          console.log("Quiz " + v.Id + " was updated.");
          this.router.navigate(["home"]);
        }, error => console.log(error));
    }
    else {
      this.http.
        put<Quiz>(url, quiz)
        .subscribe(result => {
          var v = result;
          console.log("Quiz " + v.Id + " was created.");
          this.router.navigate(["home"]);
        }, error => console.log(error));
    }
  }

  onBack() {
    this.router.navigate(["home"]);
  }
}
