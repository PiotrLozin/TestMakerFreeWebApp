import { Component, Input, Inject } from "@angular/core";
import { Quiz } from "../../interfaces/quiz";
import { ActivatedRoute, Router } from "@angular/router";
import { HttpClient } from "@angular/common/http";

@Component({
  selector: "quiz",
  templateUrl: './quiz.component.html',
  styleUrls: ['./quiz.component.css']
})

export class QuizComponent {
  quiz: Quiz | undefined;

  constructor(private activatedRoute: ActivatedRoute,
    private router: Router,
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string) {
    //Creation of an empty object on the basis of the quiz interface
    this.quiz = <Quiz>{};

    var id = +this.activatedRoute.snapshot.params["id"];
    console.log(id);
    if (id) {
      var url = "https://localhost:7136/api/quiz/" + id;
      this.http.get<Quiz>(url).subscribe(result => {
        this.quiz = result;
      }, error => console.error(error));
    }
    else {
      console.log("Wrong id - navigate to home");
      this.router.navigate(["home"]);
    }
  }
}
