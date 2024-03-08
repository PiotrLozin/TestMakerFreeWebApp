import { HttpClient } from "@angular/common/http";
import { Component, Inject, Input, OnChanges, SimpleChange, SimpleChanges } from "@angular/core";
import { Router } from "@angular/router";
import { Quiz } from "../../interfaces/quiz";
import { Question } from "../../interfaces/question";

@Component({
  selector: "question-list",
  templateUrl: './question-list.component.html',
  styleUrls: ['./question-list.component.css']
})

export class QuestionListComponent implements OnChanges {
  @Input() quiz: Quiz | undefined;
  questions: Question[];
  title: string | undefined;
  baseUrl: string;

  constructor(private http: HttpClient,
    @Inject('BASE_URL') baseUrl: string,
    private router: Router  ) {
    this.questions = [];
    this.baseUrl = "https://localhost:7136/"
  }

  ngOnChanges(changes: SimpleChanges) {
    if (typeof changes['quiz'] !== "undefined") {
      // Download information about changes included under key quiz
      var change = changes['quiz'];

      // Perform the task only if the value has changed
      if (!change.isFirstChange()) {
        // Make an http request and retrieve the result
        this.loadData()
      }
    }
  }

  loadData() {
    var url = this.baseUrl + "api/question/All/" + this.quiz?.Id;
    this.http.get<Question[]>(url).subscribe(result => {
      this.questions = result;
    }, error => console.error(error));
  }

  onCreate() {
    this.router.navigate(["/question/create", this.quiz?.Id]);
  }

  onEdit(question: Question) {
    this.router.navigate(["/question/edit", question.Id]);
  }

  onDelete(question: Question) {
    if (confirm("Do you really want to delete this question")) {
      var url = this.baseUrl + "api/question/" + question.Id;
      this.http
        .delete(url)
        .subscribe(res => {
          console.log("Question " + question.Id + " was deleted.");

          // Refresh the list of questions
          this.loadData();
        }, error => console.log(error));
    }
  }
}


